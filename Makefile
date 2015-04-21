#
# Variables
#

# Build variables
CS=mcs

# Install variables
PREFIX=/usr/local
DESTDIR_SHARE=${PREFIX}/share/generate-playlist
DESTDIR_BIN=${PREFIX}/bin
SCRIPT_LAUNCHER=${DESTDIR_BIN}/generate-playlist

#
# Build targets
#

all: build/GeneratePlaylist.exe

build/GeneratePlaylist.exe:
	mkdir -p build
	$CS -out:build/GeneratePlaylist.exe Program.cs PlaylistGenerator.cs PlaylistFormat.cs Properties/AssemblyInfo.cs

#
# Install targets
#

install: install-share install-launcher

install-share:
	mkdir -p ${DESTDIR_SHARE} ${DESTDIR_BIN}
	cp build/GeneratePlaylist.exe ${DESTDIR_SHARE}

install-launcher:
	echo "#!/bin/sh\nmono ${DESTDIR_SHARE}/GeneratePlaylist.exe" > ${SCRIPT_LAUNCHER}
	chmod +x ${SCRIPT_LAUNCHER}

clean:
	rm -rf build
