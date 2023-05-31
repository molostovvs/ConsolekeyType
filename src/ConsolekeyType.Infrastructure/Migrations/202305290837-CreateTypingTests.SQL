create table typing_tests
(
    id          integer primary key autoincrement,
    text        text,
    language_id int, --maybe store language as plain in table?
    start_time  datetime,
    end_time    datetime,
    duration    time, --remove this because we can calculate it?
    FOREIGN KEY (language_id) REFERENCES language (id)
);
