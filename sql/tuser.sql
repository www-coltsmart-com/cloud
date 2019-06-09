
DROP TABLE IF EXISTS "public"."tuser";
CREATE TABLE "public"."tuser" (
"id" int4 DEFAULT nextval('"User_id_seq"'::regclass) NOT NULL,
"UserNo" varchar(30) COLLATE "default",
"UserName" varchar(100) COLLATE "default",
"MobilePhone" varchar(20) COLLATE "default",
"RegEmall" varchar(20) COLLATE "default",
"UserType" int4 NOT NULL,
"Password" varchar(255) COLLATE "default" NOT NULL,
"RegDate" date,
"Company" varchar(255) COLLATE "default"
)
WITH (OIDS=FALSE)

;

INSERT INTO "public"."tuser" VALUES ('1', 'guest123', '刘小东', '15865289220', '328740754@qq.com', '1', 'C3B5C2ADC39DC3837D48C2BA19C2881CC2BBC3A5C396C289013F', '2019-04-09', '上海鸣驹智能科技有限公司');


ALTER TABLE "public"."tuser" ADD PRIMARY KEY ("id");
