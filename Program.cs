using System;
using System.IO;

namespace GeneratePlaylist {
	class Program {
		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name="args">The command-line arguments.</param>
		public static void Main (string[] args) {
			#region Parse options
			string directory;

			if (args.Length == 0) {
				directory = Environment.CurrentDirectory;
			} else {
				directory = args[0];
				if (!Directory.Exists(directory)) {
					Console.Error.WriteLine("Directory not found: {0}", directory);
					return;
				}
			}
			#endregion

			Program.PrintM3U8(directory);
		}

		/// <summary>
		/// Generates a M3U playlist containing and prints it.
		/// </summary>
		/// <param name="directory">Directory.</param>
		public static void PrintM3U8 (string directory) {
			PlaylistGenerator generator = new PlaylistGenerator(directory);
			string playlistContent = generator.GetPlaylist(PlaylistFormat.M3U8);

			Console.Write(playlistContent);
		}
	}
}
