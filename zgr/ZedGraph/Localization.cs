using System;
using System.Linq;
using System.Collections.Generic;

namespace ZedGraph.ZedGraph
{
	/// <summary>
	/// Description of Localization.
	/// </summary>
	public static class Localization
	{
		public static string Translate(this string id)
		{
			string locale = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
			
			System.Reflection.Assembly callingAssembly = System.Reflection.Assembly.GetCallingAssembly();
			Type[] allTypes = callingAssembly.GetTypes();
			
			string typeName = "ZedGraphLocale";
			
			if (locale != "en")
			{
				//typeName += "_en";
			}
			
			Type type = allTypes.Where(x => x.Name.Contains(typeName)).FirstOrDefault();
			
			object o = new object();
			
			System.Reflection.PropertyInfo property = type.GetProperty(id, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
			
			return property.GetValue(o, null).ToString();
		}
	}
	/*
	internal class ZedGraphLocale_de 
	{
		/// <summary>
		///   Looks up a localized string similar to Bild in Zwischenablage kopiert.
		/// </summary>
		internal static string copied_to_clip {
			get {
				return "Bild in Zwischenablage kopiert";
			}
		}
		
		/// <summary>
		///   Looks up a localized string similar to Kopieren.
		/// </summary>
		internal static string copy {
			get {
				return "Kopieren";
			}
		}
		
		/// <summary>
		///   Looks up a localized string similar to Seite einrichten....
		/// </summary>
		internal static string page_setup {
			get {
				return "Seite einrichten...";
			}
		}
		
		/// <summary>
		///   Looks up a localized string similar to Drucken....
		/// </summary>
		internal static string print {
			get {
				return "Drucken...";
			}
		}
		
		/// <summary>
		///   Looks up a localized string similar to Bild speichern als....
		/// </summary>
		internal static string save_as {
			get {
				return "Bild speichern als...";
			}
		}
		
		/// <summary>
		///   Looks up a localized string similar to Maßstab auf Standardwert setzen.
		/// </summary>
		internal static string set_default {
			get {
				return "Maßstab auf Standardwert setzen";
			}
		}
		
		/// <summary>
		///   Looks up a localized string similar to Punktwerte anzeigen.
		/// </summary>
		internal static string show_val {
			get {
				return "Punktwerte anzeigen";
			}
		}
		
		/// <summary>
		///   Looks up a localized string similar to Überschrift.
		/// </summary>
		internal static string title_def {
			get {
				return "Überschrift";
			}
		}
		
		/// <summary>
		///   Looks up a localized string similar to Alle Zoom-/Schwenkaktionen Rückgängig.
		/// </summary>
		internal static string undo_all {
			get {
				return "Alle Zoom-/Schwenkaktionen Rückgängig";
			}
		}
		
		/// <summary>
		///   Looks up a localized string similar to Letzte Schwenkaktion Rückgängig.
		/// </summary>
		internal static string unpan {
			get {
				return "Letzte Schwenkaktion Rückgängig";
			}
		}
		
		/// <summary>
		///   Looks up a localized string similar to Letzte Zoomaktion Rückgängig.
		/// </summary>
		internal static string unzoom {
			get {
				return "Letzte Zoomaktion Rückgängig";
			}
		}
		
		/// <summary>
		///   Looks up a localized string similar to X-Achse.
		/// </summary>
		internal static string x_title_def {
			get {
				return "X-Achse";
			}
		}
		
		/// <summary>
		///   Looks up a localized string similar to Y-Achse.
		/// </summary>
		internal static string y_title_def {
			get {
				return "Y-Achse";
			}
		}
	}
	*/
	
	internal class ZedGraphLocale
	{
		/// <summary>
		///   Looks up a localized string similar to Bild in Zwischenablage kopiert.
		/// </summary>
		internal static string copied_to_clip {
			get {
				return "Image copied to clipboard";
			}
		}
		
		/// <summary>
		///   Looks up a localized string similar to Kopieren.
		/// </summary>
		internal static string copy {
			get {
				return "Copy";
			}
		}
		
		/// <summary>
		///   Looks up a localized string similar to Seite einrichten....
		/// </summary>
		internal static string page_setup {
			get {
				return "Page Setup...";
			}
		}
		
		/// <summary>
		///   Looks up a localized string similar to Drucken....
		/// </summary>
		internal static string print {
			get {
				return "Print...";
			}
		}
		
		/// <summary>
		///   Looks up a localized string similar to Bild speichern als....
		/// </summary>
		internal static string save_as {
			get {
				return "Save Image As...";
			}
		}
		
		/// <summary>
		///   Looks up a localized string similar to Maßstab auf Standardwert setzen.
		/// </summary>
		internal static string set_default {
			get {
				return "Set Scale to Default";
			}
		}
		
		/// <summary>
		///   Looks up a localized string similar to Punktwerte anzeigen.
		/// </summary>
		internal static string show_val {
			get {
				return "Show Point Values";
			}
		}
		
		/// <summary>
		///   Looks up a localized string similar to Überschrift.
		/// </summary>
		internal static string title_def {
			get {
				return "Title";
			}
		}
		
		/// <summary>
		///   Looks up a localized string similar to Alle Zoom-/Schwenkaktionen Rückgängig.
		/// </summary>
		internal static string undo_all {
			get {
				return "Undo All Zoom/Pan";
			}
		}
		
		/// <summary>
		///   Looks up a localized string similar to Letzte Schwenkaktion Rückgängig.
		/// </summary>
		internal static string unpan {
			get {
				return "Un-Pan";
			}
		}
		
		/// <summary>
		///   Looks up a localized string similar to Letzte Zoomaktion Rückgängig.
		/// </summary>
		internal static string unzoom {
			get {
				return "Un-Zoom";
			}
		}
		
		/// <summary>
		///   Looks up a localized string similar to X-Achse.
		/// </summary>
		internal static string x_title_def {
			get {
				return "X Axis";
			}
		}
		
		/// <summary>
		///   Looks up a localized string similar to Y-Achse.
		/// </summary>
		internal static string y_title_def {
			get {
				return "Y Axis";
			}
		}
	}
}
