CREATE TABLE registered_user
(
  registered_user_id     bigserial   NOT NULL UNIQUE PRIMARY KEY,
  login                  varchar(20) NOT NULL UNIQUE,
  password               varchar(30) NOT NULL,
  email                  varchar(50) NOT NULL,
  full_name              varchar(40),
  phone                  varchar(15),
  registration_date_time timestamp   NOT NULL,
  email_confirmed        boolean,
  avatar_url             varchar
);

create table company
(
  company_id bigserial   not null unique primary key,
  owner_id   bigserial   not null
    constraint company_owner_id_fk
      references registered_user,
  name       varchar(40) not null unique,
  email      varchar(40) not null
);

CREATE TABLE administrators
(
  administrator_id   bigserial NOT NULL UNIQUE PRIMARY KEY,
  registered_user_id bigserial not null
    constraint registered_user_id_fk
      references registered_user,
  company_id         bigserial not null
    constraint company_id_fk
      references company
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