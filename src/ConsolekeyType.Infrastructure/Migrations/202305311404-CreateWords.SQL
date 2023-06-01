create table words
(
    word        varchar(60), --in english the longest word is 45 letters long, in russian 55 (both for chemical stuff)
    language_id int,
    FOREIGN KEY (language_id) REFERENCES language (id)
);