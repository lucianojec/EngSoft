CREATE TABLE "public"."project_metrics_integration" (
	"integration_id" BIGSERIAL, 
	"project_id" integer NOT NULL,
	"executed_at" timestamp(6), 	
	"coverage" numeric(38, 20),
	"total" integer,
	"percentege" numeric(38,20),
	CONSTRAINT "integration_pk" PRIMARY KEY ("integration_id"), 
	FOREIGN KEY ("project_id")
		REFERENCES "public"."projects" ("project_id")
)