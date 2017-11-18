CREATE TABLE FolderTree (
    id        INTEGER PRIMARY KEY
                      NOT NULL,
    parentid  INTEGER NOT NULL,
    name      TEXT    NOT NULL,
    meta      TEXT    NOT NULL
);

CREATE TABLE Files (
    id        INTEGER PRIMARY KEY
                      NOT NULL,
    folderid  INTEGER NOT NULL,
    name      TEXT    NOT NULL
);

CREATE TABLE FileMetadata (
    id        INTEGER PRIMARY KEY
                      NOT NULL,
    fileid   INTEGER NOT NULL
);

CREATE TABLE Metadata (
	id        INTEGER PRIMARY KEY
                      NOT NULL,
	filemetaid       INTEGER NOT NULL,
    metakey          TEXT    NOT NULL,
	metavalue        TEXT    NOT NULL
);


