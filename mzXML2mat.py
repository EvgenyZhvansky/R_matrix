# import sys
import numpy as np
from pyteomics import mzxml as pymz
import xml.etree.ElementTree as ET
from tkinter import Tk
from tkinter.filedialog import askopenfilenames
import scipy.io as io


def get_scan_count(tree_local):
    scan_count=0
    for (event, elem) in tree_local:
        if 'msRun' in elem.tag:
            scan_count = int(elem.attrib['scanCount'])
            return scan_count
			
def binning(mz, spectr, mzs_net,mz_len):
    index = np.digitize(mz, mzs_net)
    return np.bincount(index, weights=spectr,minlength=mz_len+2)[1:-1]
	
def median_filtering(filenames, scans_count, n_median, mzs_net,mz_len):
    scans_count=scans_count//n_median
    spectra_binned=np.zeros((scans_count.sum(),mz_len))
    k=0
    if n_median==1:
        for filename in filenames:
            reader = pymz.read(filename)
            print(filename)
            for tree in reader:
                mz = tree['m/z array']
                spectr = tree['intensity array']
                spectra_binned[k,:]=binning(mz,spectr,mzs_net,mz_len)
                k+=1
    else:
        spectra_to_median=np.zeros((n_median,mz_len))
        for i, filename in enumerate(filenames):
            reader = pymz.read(filename)
            print(filename)
            for j in range(scans_count[i]):
                for t in range(n_median):
                    tree=next(reader)
                    mz = tree['m/z array']
                    spectr = tree['intensity array']
                    spectra_to_median[t,:]=binning(mz,spectr,mzs_net,mz_len)
                spectra_binned[k,:]=np.median(spectra_to_median,axis=0)
                k+=1
    return spectra_binned

def calc_median(filenames,whole_scans_count, mzmin, mzmax, mz_bin,mem_in_GB):
    mz_len=int(1+np.round((mzmax-mzmin)/mz_bin))

    #estimation of moving median parameter
    n_median=int(whole_scans_count*mz_len*8/mem_in_GB/2**30)
    if n_median<1:
        n_median=1
    else:
        n_median=int(2*np.floor(n_median/2.0)+1)
        while int(np.sum(np.floor(scans_count/n_median))*mz_len*8/mem_in_GB/2**30)>=1:
            n_median-=2
        n_median+=2
    return n_median
	
def calc_scans_count(filenames, mzmin, mzmax, mz_bin, n_median,scans_count,mem_in_GB):
    mz_len=int(1+np.round((mzmax-mzmin)/mz_bin))

    if np.sum(scans_count//n_median)*mz_len*8/mem_in_GB/2**30>1:
        scans_count=scans_count//n_median
        over_scans=np.ceil(mem_in_GB*2**30/mz_len/8)-np.sum(scans_count)
        for i in range(over_scans):
            scans_count[np.argmax(scans_count)]-=1
        scans_count*=n_median
    return scans_count,n_median
	
def read_and_convert_data(filenames, mzmin=200, mzmax=1000, mz_bin=0.1, n_median=None, GB_max=2):
    mz_len=int(1+np.round((mzmax-mzmin)/mz_bin))
    scans_count=np.zeros(len(filenames), dtype=np.int32)
    for i,filename in enumerate(filenames):
        scans_count[i]=get_scan_count(ET.iterparse(filename, events=("start","end")))
    whole_scans_count=scans_count.sum()
    
    if n_median==None:
        n_median=calc_median(filenames, whole_scans_count, mzmin, mzmax, mz_bin,GB_max)
    else:
        n_median = int(2 * np.floor(n_median / 2.0) + 1)
        scans_count,n_median=calc_scans_count(filenames, mzmin, mzmax, mz_bin, n_median,scans_count,GB_max)

    print("Median width is set to ", str(n_median), '\n')

    whole_median_scans_count=int(np.sum(scans_count//n_median))
    
    mz_array=mzmin+mz_bin*np.array(range(mz_len))
    mzs_net=mzmin-mz_bin/2+mz_bin*np.array(range(mz_len+1))

    spectra_binned=np.zeros((whole_median_scans_count,mz_len))
    
    return mz_array, median_filtering(filenames, scans_count, n_median, mzs_net, mz_len),scans_count

def create_data_var(filenames, mz_array,spectra,scans_count):
    for filename in filenames:
        outs = filename.split("/")
        break
    out_filename_mat=''
    for i in range(len(outs)-1):
        out_filename_mat +=  outs[i] +  "/"
    out_filename_mat += 'data.mat'

    data={}

    data['mz'] = mz_array
    data['spectra'] = spectra

    i=0
    N=scans_count.sum()
    arr=np.zeros(N)
    for id,el in enumerate(scans_count):
        arr[i:i+el]=id
        i+=el

    data['filenames']=np.empty((len(filenames),), dtype=np.object)

    filenames_only=filenames
    for i,filename in enumerate(filenames):
        outs = filename.rsplit("/",maxsplit=1)
        data['filenames'][i]=outs[1]

    data['scan_of_file']=arr

    return out_filename_mat, data

if __name__ == '__main__':
    root = Tk()
    root.withdraw()
    filenames = askopenfilenames(parent=root, filetypes=[("mzXML files", ".mzXML")])
    if filenames != '':
        mzmin = 200
        mzmax = 1000
        mzbin=0.1
        n_median=None
        GB_max=2

        print("Skip any step to use the default value")

        try:
            mzmin=float(input("Enter the m/z min (default is 200): "))
        except ValueError:
            print('Default will be used. Enter only digits. For decimals use " . " ')

        try:
            mzmax=float(input("\nEnter the m/z max (default is 1000): "))
        except ValueError:
            print('Default will be used. Enter only digits. For decimals use " . " ')

        try:
            mzbin=float(input("\nEnter the m/z bin width (default is 0.1): "))
        except ValueError:
            print('Default will be used. Enter only digits. For decimals use " . " ')

        try:
            n_median=int(input("\nEnter the median width value (enter 1 for no median filtration, skip for automated width chosing): "))
        except ValueError:
            print("Default median parameter will be calculated automatically. Enter only digits")

        try:
            GB_max=int(input("\nEnter the RAM space max (in GB): "))
            if GB_max<1:
                GB_max=2
                print('Too small value. Default will be used (2).')
        except ValueError:
            print('Default will be used (2). Enter only digits.')

        mz_array,spectra,scans_count=read_and_convert_data(filenames,
                                                           mzmin=mzmin,
                                                           mzmax=mzmax,
                                                           mz_bin=mzbin,
                                                           n_median=n_median,
                                                           GB_max=GB_max)
        out_filename_mat, data = create_data_var(filenames, mz_array,spectra,scans_count)
        io.savemat(out_filename_mat, {'data':data})
        print(out_filename_mat)
    input("Press any key to exit... ")
