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
	public class FileSystemHelper
	{
		#region Fields

		private const string _NewDirectoryName = "Input\\";
		private const string _BaseDirectory = "C:\\Users\\Public\\Pictures\\O-SELECT\\Simulation\\";
		private const string _NewDirectory = _BaseDirectory + _NewDirectoryName;
		private const string _PatternWithExtension = "???.png";

		#endregion

		#region Methods

		/// <summary>
		/// Load (copy) images.
		/// </summary>
		public static void LoadImages()
		{
			string sourceFileNameAndPath = ChooseFolderWithImages();
			if (sourceFileNameAndPath != null)
			{
				string sourcePath = Path.GetDirectoryName(sourceFileNameAndPath);
				List<string> imageList = FindImagesWithPattern(sourceFileNameAndPath, sourcePath);
				CreateFileWithNumberOfImages(imageList, _NewDirectory);
				CopyImages(imageList, sourcePath, _NewDirectory);
			}
		}

		/// <summary>
		/// Open file dialog window and let the user select the file
		/// </summary>
		/// <returns>File name with a path</returns>
		public static string ChooseFolderWithImages()
		{
			string sourceFileNameAndPath = null;
			var dlg = new OpenFileDialog() 
			{
				DefaultExt = ".png",
				Filter = "Portable network graphics files (*.png)|*.png",
				RestoreDirectory = true
			};
			// Show open file dialog box

			if (dlg.ShowDialog() == DialogResult.OK)
			{
				sourceFileNameAndPath = dlg.FileName;
			}
			return sourceFileNameAndPath;
		}

		/// <summary>
		/// Copy images from source path to destination path
		/// </summary>
		/// <param name="imageList">List with the image names to be copied</param>
		/// <param name="sourcePath">Source path</param>
		/// <param name="destinationPath">Destination path</param>
		public static void CopyImages(IEnumerable<string> imageList, string sourcePath, string destinationPath)
		{
			try
			{
				foreach (string file in imageList)
				{
					// Remove path from the file name.
					string fName = file.Substring(sourcePath.Length + 1);

					// Use the Path.Combine method to safely append the file name to the path.
					// Will overwrite if the destination file already exists.
					File.Copy(Path.Combine(sourcePath, fName), Path.Combine(destinationPath, fName), true);
				}
			}
			catch (Exception exception)
			{
				throw new Exception(exception.ToString());
			}
		}

		/// <summary>
		/// Finds file name patterns in the folder selected by the user. File name consists of Prefix_Postfix.png. 
		/// In order to display the files one after another the files need to be PNG images 
		/// and follow the pattern: Prefix_001.png, Prefix_002.png, Prefix_003.png to Prefix_999.png. 
		/// Any other postfix configuration will only display one image that was selected by the user.
		/// </summary>
		/// <param name="fileName">File name selected by the user</param>
		/// <param name="sourcePath">Directory selected by the user</param>
		private static List<string> FindImagesWithPattern(string fileName, string sourcePath)
		{
			var imageList = new List<string>();

			var fileNameLength = fileName.Length;
			var firstFileFileName = Path.GetFileName(fileName);
			var firstFileNameLength = firstFileFileName.Length;
			int endPreFixPosition = firstFileNameLength - 7; // 4 positions for extension, 3 for postfix

			if (firstFileNameLength < 7)
			{
				// Copy only one file (chosen by the user) as the pattern does not have three numbers
				imageList.Clear();
				imageList.Add(fileName);
			}
			else if (firstFileNameLength == 7)
			{
				// If the length of the filename with extension is exactly 7 then check if the filename consists only of numbers.
				var fileNameWithoutExtension = firstFileFileName.Remove(3);
				string preFix = firstFileFileName.Remove(endPreFixPosition);

				if (IsDigitsOnly(fileNameWithoutExtension))
				{
					imageList = Directory.GetFiles(sourcePath, preFix + _PatternWithExtension).ToList();
					imageList.Sort();
					int indexOfLast = imageList[0].Length - 5;
					char lastCharacter = imageList[0][indexOfLast];

					if (lastCharacter != '1')
					{
						// Sequence pattern not recognized copy only one chosen image
						imageList.Clear();
						imageList.Add(fileName);
					}
					// It is assumed that sequence must start with 1 as the last digit (number,number,1) 
				}
				else
				{
					// Sequence pattern not recognized (consists of letters) copy only one chosen image
					imageList.Clear();
					imageList.Add(fileName);
				}
			}
			else
			{
				// For longer file names find prefix
				string preFix = firstFileFileName.Remove(endPreFixPosition);
				// Additionally filters the list in order to chose sequence with the same file length
				imageList = Directory.GetFiles(sourcePath, preFix + _PatternWithExtension).Where(x => x.Length == fileNameLength).ToList();

				if (imageList.Count != 1)
				{
					imageList.Sort();
					int indexOfLast = imageList[0].Length - 5;
					char lastCharacter = imageList[0][indexOfLast];

					if (lastCharacter != '1')
					{
						imageList.Clear();
						imageList.Add(fileName);
					}
				}
				else
				{
					imageList.Clear();
					imageList.Add(fileName);
				}
			}

			return imageList;
		}
		/// <summary>
		/// Checks if string consists only of numbers
		/// </summary>
		/// <param name="fileName">String to be checked</param>
		/// <returns>True if string consists of numbers, false otherwise</returns>
		private static bool IsDigitsOnly(string fileName)
		{
			foreach (char c in fileName)
			{
				if (c < '0' || c > '9')
					return false;
			}
			return true;
		}

		/// <summary>
		/// Creates helper txt file with the number of the images contained in _NewDirectory.
		/// The number is the name of the created file. This file is used to decrease 
		/// the time thread blocking in DMEOpticsMachine() in the IsCopyingDone() method. 
		/// </summary>
		/// <param name="imageList">List with the file files in the directory</param>
		/// <param name="path">Path where the file should be stored</param>
		private static void CreateFileWithNumberOfImages(List<string> imageList, string path)
		{
			string numberOfImages = imageList.Count.ToString();

			TextWriter tw = new StreamWriter(path + numberOfImages + ".txt", true);
			tw.Close();
		}

		#endregion
	}
}
