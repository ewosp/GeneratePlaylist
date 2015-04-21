#
# Variables
#

# Build variables
CS=mcs

# Install variables
PREFIX=/usr/local
DESTDIR_LIB=${PREFIX}/lib/generate-playlist
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

install: install-lib install-launcher

install-lib:
	mkdir -p ${DESTDIR_LIB} ${DESTDIR_BIN}
	cp build/GeneratePlaylist.exe ${DESTDIR_LIB}

install-launcher:
	echo "#!/bin/sh\nmono ${DESTDIR_LIB}/GeneratePlaylist.exe" > ${SCRIPT_LAUNCHER}
	chmod +x ${SCRIPT_LAUNCHER}

clean:
	rm -rf build
