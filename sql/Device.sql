
DROP TABLE IF EXISTS "public"."Device";
CREATE TABLE "public"."Device" (
"id" int4 DEFAULT nextval('"Device_id_seq"'::regclass) NOT NULL,
"DeviceId" varchar(50) COLLATE "default" NOT NULL,
"DeviceType" varchar(40) COLLATE "default" NOT NULL,
"IsGetway" bool NOT NULL,
"DeviceName" varchar(40) COLLATE "default" NOT NULL,
"IsOnline" bool NOT NULL,
"InDate" date NOT NULL,
"UserOwn" varchar(32) COLLATE "default" NOT NULL,
"Gps" varchar(40) COLLATE "default" NOT NULL
)
WITH (OIDS=FALSE)

;

ALTER TABLE "public"."Device" ADD PRIMARY KEY ("id");
