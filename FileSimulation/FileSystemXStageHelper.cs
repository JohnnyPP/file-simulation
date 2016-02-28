#region Copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * *
 * * File name :"FileSystemHelper.cs"
 * * Company   : 
 * * (c) 
 * * <author>JohnnyP</author>
 * * * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

#endregion

namespace FileSimulation
{
	/// <summary>
	/// Helper class which contains static helper methods for accessing  the file system.
	/// </summary>
	public class FileSystemXStageHelper : FileSystemHelper
	{
		#region Fields

		private const string _NewDirectoryName = "Input\\";
		private const string _BaseDirectory = "C:\\Users\\Public\\Pictures\\O-SELECT\\Simulation\\";
		private const string _NewDirectory = _BaseDirectory + _NewDirectoryName;
		private const string _PatternWithExtension = "???.png";

		#endregion

		private static class ImageNames 
		{
			public const string LeftImage = "_l.png";
			public const string RightImage = "_r.png";
		}

		#region Methods

		/// <summary>
		/// Load (copy) images.
		/// </summary>
		public static new void LoadImages()
		{
			string sourceFileNameAndPath = ChooseFolderWithImages();
			if (sourceFileNameAndPath != null)
			{
				string sourcePath = Path.GetDirectoryName(sourceFileNameAndPath);
				List<string> imageList = FindImagesWithPattern(sourceFileNameAndPath);
				CopyImages(imageList, sourcePath, _NewDirectory);
			}
		}

		/// <summary>
		/// Finds file name patterns in the folder selected by the user. File name consists of Prefix_Postfix.png. 
		/// Clicked Postfix == *_r.png or *_l.png 
		/// Clicked filename is split to Prefix and Postfix based on saved postfix
		/// If file with complementary postfix exists both files are stored in the list
		/// </summary>
		/// <param name="fileNameWithPath">File name selected by the user</param>
		/// <returns>List with images to be copied into Input folder</returns
		private static List<string> FindImagesWithPattern(string fileNameWithPath)
		{
			var imageList = new List<string>();
			string complementaryPostfix;
			string clickedPostfix = fileNameWithPath.Substring(fileNameWithPath.Length - 6);
			string[] stringSeparators = new string[] { clickedPostfix };
			string[] prefix = fileNameWithPath.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

			if (clickedPostfix == ImageNames.RightImage)
			{
				complementaryPostfix = ImageNames.LeftImage;
			}
			else
			{
				complementaryPostfix = ImageNames.RightImage; 
			}

			if (File.Exists(prefix[0] + complementaryPostfix))
			{
				imageList.Add(prefix[0] + clickedPostfix);
				imageList.Add(prefix[0] + complementaryPostfix);
			}
			else
			{
				Console.WriteLine("File with {0} postfix not found", complementaryPostfix);
			}

			return imageList;
		}

		/// <summary>
		/// This should find prefix and both postfixes and put the result to dictionary
		/// e.g. <LeftImage, path + 001_l.png> and <RightImage, path + 001_r.png>
		/// If the DMEcommand == 0.0 then display LeftImage otherwise RightImage
		/// </summary>
		/// <param name="fileNameWithPath"></param>
		/// <param name="DMECommand"></param>
		/// <returns></returns>
		private static Dictionary<string, string> FindImageToDisplay(string fileNameWithPath, string DMECommand)
		{
			var imageDictionary = new Dictionary<string, string>();

			return imageDictionary;
		}
		#endregion
	}
}
