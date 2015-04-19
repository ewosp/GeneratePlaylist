using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace GeneratePlaylist {
	public class PlaylistGenerator {
		/// <summary>
		/// The directory where the multimedia files are located.
		/// </summary>
		private string multimediaFilesDirectory;

		/// <summary>
		/// Initializes a new instance of the <see cref="GeneratePlaylist.PlaylistGenerator"/> class.
		/// </summary>
		/// <param name="directory">The directory where the multimedia files are located</param>
		public PlaylistGenerator (string directory) {
			this.MultimediaFilesDirectory = directory;
		}

		/// <summary>
		/// Gets or sets the directory where the multimedia files are located.
		/// </summary>
		/// <value>The directory.</value>
		/// <exception cref="System.ArgumentException">Throws if the directory doesn't exist.</exception>
		public string MultimediaFilesDirectory {
			get {
				return multimediaFilesDirectory;
			}
			set {
				if (!Directory.Exists(value)) {
					throw new ArgumentException ("Directory doesn't exist", "folder");
				}
				multimediaFilesDirectory = value;
			}
		}

		/// <summary>
		/// Gets the multimedia files.
		/// </summary>
		/// <returns>The multimedia files.</returns>
		public string[] GetMultimediaFiles () {
			return GetMultimediaFiles(true);
		}

		/// <summary>
		/// Gets the playlist.
		/// </summary>
		/// <returns>The playlist.</returns>
		public string GetPlaylist (PlaylistFormat format) {
			switch (format) {
				case PlaylistFormat.M3U8:
					return this.GetM3U8Playlist();

				case PlaylistFormat.PLS:
					return this.GetPLSPlaylist();

				default:
					throw new NotImplementedException("Generate a playlist in this format isn't implemented.");
			}
		}

		/// <summary>
		/// Gets the playlist in UTF-8 extended format.
		/// </summary>
		/// <returns>The playlist in UTF-8 extended M3U format.</returns>
		public string GetM3U8Playlist () {
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("#EXTM3U");
			string[] files = GetMultimediaFiles();
			foreach (string file in files) {
				sb.AppendLine(file);
			}
			return sb.ToString();
		}

		/// <summary>
		/// Gets the playlist in PLS format.
		/// </summary>
		/// <returns>The playlist in PLS format.</returns>
		public string GetPLSPlaylist () {
			//Reference: http://forums.winamp.com/showthread.php?threadid=65772
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("[playlist]");

			string[] files = GetMultimediaFiles();
			int counter = 0;
			foreach (string file in files) {
				counter++;

				sb.AppendFormat("File{0}={1}", counter, file);
				sb.AppendLine();

				sb.AppendFormat("Length{0}=-1", counter);
				sb.AppendLine();
			}

			sb.AppendFormat("NumberOfEntries={0}", counter);
			sb.AppendLine();

			sb.AppendLine("Version=2");
			return sb.ToString();
		}

		/// <summary>
		/// Gets the multimedia files.
		/// </summary>
		/// <returns>The multimedia files.</returns>
		/// <param name="lookInSubDirectories">If set to <c>true</c>, look in subdirectories.</param>
		public string[] GetMultimediaFiles (bool lookInSubDirectories) {
			return GetMultimediaFilesInDirectory(
				this.multimediaFilesDirectory,
				lookInSubDirectories
			);
		}

		/// <summary>
		/// Gets the multimedia files in the specified directory.
		/// </summary>
		/// <param name="directory">The directory where to find multimedia files.</param>
		/// <param name="lookInSubDirectories">if set to <c>true</c>, looks in subdirectories.</param>
		/// <returns>
		/// An array containing themultimedia files found in directory.
		/// </returns>
		public static string[] GetMultimediaFilesInDirectory (string directory, bool lookInSubDirectories) {
			HashSet<string> files = new HashSet<string>();
			DirectoryInfo di = new DirectoryInfo(directory);

			//Looks the files in the current directories, and gets an unique list of multimedia files
			try {
				foreach (FileInfo fi in di.GetFiles()) {
					if (IsMultimediaExtension(fi.Extension)) {
						files.Add(fi.FullName);
					}
				}
			} catch (UnauthorizedAccessException) {
			}

			//In recursive mode, looks also in subdirectories
			if (lookInSubDirectories) {
				try {
					foreach (DirectoryInfo subDi in di.GetDirectories()) {
						files.UnionWith(GetMultimediaFilesInDirectory(subDi.FullName, true));
					}
				} catch (UnauthorizedAccessException) {
				}
			}

			string[] result = new string[files.Count];
			files.CopyTo(result);
			return result;
		}

		/// <summary>
		/// The known multimedia extensions.
		/// </summary>
		public static List<string> multimediaExtensions = new List<string>(new string[] {
			".avi",
			".flac", 
			".m4a",
			".mp3",
			".mpc",
			".mpg",
			".ogg",
			".wav",
			".wma",
		});

		/// <summary>
		/// Determines if the specified extension matches a known multimedia one.
		/// </summary>
		/// <returns><c>true</c> if is multimedia extension the specified extension; otherwise, <c>false</c>.</returns>
		/// <param name="extension">Extension.</param>
		public static bool IsMultimediaExtension (string extension) {
			return multimediaExtensions.Contains(extension.ToLower());
		}
	}
}