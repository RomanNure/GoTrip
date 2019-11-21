CREATE TABLE registered_user
(
  registered_user_id     bigserial   NOT NULL UNIQUE PRIMARY KEY,
  login                  varchar(20) NOT NULL UNIQUE,
  password               varchar(30) NOT NULL,
  email                  varchar(50) NOT NULL UNIQUE,
  full_name              varchar(40),
  phone                  varchar(15),
  registration_date_time timestamp   NOT NULL,
  email_confirmed        boolean,
  avatar_url             varchar,
  description            varchar
);

create table company
(
  company_id bigserial   not null unique primary key,
  owner_id   bigserial   not null
    constraint company_owner_id_fk
      references registered_user,
  name       varchar(40) not null unique,
  email      varchar(40) not null,
  phone      varchar(20),
  address    varchar(255),
  domain     varchar(255)
);

CREATE TABLE administrators
(
  administrator_id   bigserial NOT NULL UNIQUE PRIMARY KEY,
  registered_user_id bigserial not null
    constraint registered_user_id_fk
      references registered_user,
  company_id         bigserial not null
    constraint company_id_fk
      references company,
  unique (registered_user_id, company_id)
);

create table notification_types
(
  notification_type_id bigserial   not null unique primary key,
  name                 varchar(20) not null unique
);

create table notifications
(
  notification_id      bigserial   NOT NULL UNIQUE PRIMARY KEY,
  notification_type_id bigserial   not null
    constraint notification_type_id_fk
      references notification_types,
  registered_user_id   bigserial   not null
    constraint registered_user_id_fk
      references registered_user,
  topic                varchar(50) not null,
  text                 varchar     not null
);

create table guide
(
  guide_id              bigserial not null unique primary key,
  registered_user_id    bigserial not null
    constraint guide_registered_user_id_fk
      references registered_user,
  wanted_tours_keywords varchar
);

create table tours
(
  tour_id          bigserial      not null unique primary key,
  administrator_id bigserial
    constraint tours_administrator_id_fk
      references administrators,
  name             varchar(70)    not null,
  description      varchar        not null,
  price_per_person numeric(10, 2) not null,
  main_picture_url varchar,
  start_date_time  timestamp      not null,
  finish_date_time timestamp      not null,
  max_participants integer,
  location         varchar        not null,
  guide_id bigserial
    constraint tours_guides_id_fk
    references guide
);

create table tour_photos
(
  tour_photo_id bigserial not null unique primary key,
  photo_url     varchar   not null,
  tour_id       bigserial not null
    constraint tours_photos_tours_id_fk
      references tours
);

create table participating
(
  participating_id   bigserial not null unique primary key,
  registered_user_id bigserial not null
    constraint participating_user_id_fk
      references registered_user,
  tour_id            bigserial not null
    constraint participating_tour_id_fk
      references tours,
  tour_rate          integer            default -1,
  ticket_hash        varchar   not null default '-1',
  guide_rate         integer            default -1,
  unique (registered_user_id, tour_id)
);

SELECT tours.*, administrators.*, tour_photos.*, participating.*
FROM tours inner join administrators on tours