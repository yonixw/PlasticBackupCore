CREATE TABLE FolderTree (
    id        INTEGER PRIMARY KEY
                      NOT NULL,
    parentid  INTEGER NOT NULL,
    name      TEXT    NOT NULL
);

CREATE TABLE Files (
    id        INTEGER PRIMARY KEY
                      NOT NULL,
    folderid  INTEGER NOT NULL,
    name      TEXT    NOT NULL
);

-- More information, when source is either a file or a folder.
-- One file can have 2 meta items (e.g. all win pc have some C:\Windows\...\?.dll alike)
-- New path of file is only determined by it's hash metadata. No duplicates!

CREATE TABLE MetadataItems (
    id        INTEGER PRIMARY KEY
                      NOT NULL,
	isFolder  BOOLEAN NOT NULL,
    sourceid  INTEGER NOT NULL -- Search in folder\file table based on `isFolder`
);

CREATE TABLE MetadataValues (
	id        INTEGER PRIMARY KEY
                      NOT NULL,
	itemid           INTEGER NOT NULL,
    metakey          TEXT    NOT NULL,
	metavalue        TEXT    NOT NULL
);


