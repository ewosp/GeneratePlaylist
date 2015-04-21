all: build/GeneratePlaylist.exe

build/GeneratePlaylist.exe:
	mkdir -p build
	mcs -out:build/GeneratePlaylist.exe Program.cs PlaylistGenerator.cs PlaylistFormat.cs Properties/AssemblyInfo.cs

clean:
	rm -rf build
