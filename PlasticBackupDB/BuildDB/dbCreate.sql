CREATE TABLE FolderTree (
    id        INTEGER PRIMARY KEY
                      NOT NULL,
    parent_id INTEGER NOT NULL,
    name      TEXT    NOT NULL,
    meta      TEXT    NOT NULL
);

CREATE TABLE Files (
    id        INTEGER PRIMARY KEY
                      NOT NULL,
    folder_id INTEGER NOT NULL,
    name      TEXT    NOT NULL,
    meta      TEXT    NOT NULL
);


INSERT INTO FolderTree (id,parent_id,name, meta)
		VALUES (0, 0,'ROOT', '<>' );
