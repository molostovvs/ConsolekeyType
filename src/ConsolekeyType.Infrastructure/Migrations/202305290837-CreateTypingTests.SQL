create table typing_tests
(
    id          integer primary key autoincrement,
    text        text,
    language_id int,
    start_time  datetime,
    end_time    datetime,
    duration    time, --remove this because we can calculate it?
    cpm         real,
    wpm         real,
    FOREIGN KEY (language_id) REFERENCES language (id)
);
