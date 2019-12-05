select * from project_metrics pm
--delete from project_metrics

/*
ALTER TABLE public.project_metrics ADD COLUMN major_issues int4 null;
ALTER TABLE public.project_metrics ADD COLUMN minor_issues int4 null;
ALTER TABLE public.project_metrics ADD COLUMN code_smells int4 null;
ALTER TABLE public.project_metrics ADD COLUMN vulnerabilities int4 null;
ALTER TABLE public.project_metrics ADD COLUMN critical_issues int4 null;
*/

ALTER TABLE public.project_metrics ADD COLUMN critical_issues int4 null;

select * from project_metrics pm

select 
	projects.id, 
    projects.kee, 
    projects.name,
    snapshots.build_date, 
    project_measures.analysis_uuid, 
    project_measures.metric_id, 
    project_measures.value 
  from 
    projects 
      left join snapshots on 
        projects.uuid = snapshots.component_uuid 
        and snapshots.build_date >= 1564628400000 
      left join project_measures on 
        project_measures.component_uuid = snapshots.component_uuid and 
        project_measures.analysis_uuid = snapshots.uuid  and
        project_measures.metric_id in (1, 37, 39, 41, 99, 100, 113, 117, 98) 
 where projects.id =156723 
   and projects.tags like '%#saj6%' 
 order by build_date, analysis_uuid, project_measures.metric_id;

 select* from metrics m
 
 
select * from project_metrics pm
delete from project_metrics

/*
ALTER TABLE public.project_metrics ADD COLUMN major_issues int4 null;
ALTER TABLE public.project_metrics ADD COLUMN minor_issues int4 null;
ALTER TABLE public.project_metrics ADD COLUMN code_smells int4 null;
ALTER TABLE public.project_metrics ADD COLUMN vulnerabilities int4 null;
*/

select * from project_metrics pm