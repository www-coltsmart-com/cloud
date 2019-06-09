
DROP TABLE IF EXISTS "public"."DeviceNet";
CREATE TABLE "public"."DeviceNet" (
"id" int4 DEFAULT nextval('"DeviceNet_id_seq"'::regclass) NOT NULL,
"DeviceId" int4 NOT NULL,
"NetFlow" float4 NOT NULL
)
WITH (OIDS=FALSE)

;

