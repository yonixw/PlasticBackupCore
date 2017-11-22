-- Autoincrement and sqlite_sequence https://stackoverflow.com/a/2217015/1997873

CREATE TABLE FolderTree (
    id        INTEGER PRIMARY KEY AUTOINCREMENT
                      NOT NULL,
    parentid  INTEGER NOT NULL,
    name      TEXT    NOT NULL
);

CREATE TABLE Files (
    id        INTEGER PRIMARY KEY AUTOINCREMENT
                      NOT NULL,
    folderid  INTEGER NOT NULL,
    name      TEXT    NOT NULL
);

-- More information, when source is either a file or a folder.
-- One file can have 2 meta instance (e.g. all win pc have some C:\Windows\...\?.dll alike)
-- New path of file is only determined by it's hash metadata. No duplicates!

CREATE TABLE MetadataInstances (
    id        INTEGER PRIMARY KEY AUTOINCREMENT
                      NOT NULL,
	isFolder  BOOLEAN NOT NULL,
    sourceid  INTEGER NOT NULL -- Search in folder\file table based on `isFolder`
);

-- Each Instance have <key,value> pairs of metadata.

CREATE TABLE MetadataItems (
	id        INTEGER PRIMARY KEY AUTOINCREMENT
                      NOT NULL,
	instanceid       INTEGER NOT NULL,
    metakey          TEXT    NOT NULL,
	metavalue        TEXT    NOT NULL
);


