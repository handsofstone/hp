--select Id from LineupPlayer where IntervalId = 40 and TeamId = 2
--UNION ALL
--SELECT 0
--WHERE NOT EXISTS (select 1 from LineupPlayer where IntervalId = 40 and TeamId = 2);

--select * from LineupPlayer where IntervalId = 40 and TeamId = 2
--UNION ALL
--select *
--from
--(select lp.*
--from LineupPlayer lp
--right join RosterPlayer rp on lp.PlayerId = rp.PlayerId and lp.TeamId = rp.TeamId
--where lp.TeamId = 2 and lp.IntervalId =
--(select top 1 (lp.IntervalId)
--from LineupPlayer lp 
--join Interval i on lp.IntervalId = i.Id
--where lp.TeamId = 2 and i.endDate <= (select EndDate from Interval where Id = 40)
--order by i.EndDate desc)) t
--WHERE NOT EXISTS (select 1 from LineupPlayer where IntervalId = 40 and TeamId = 2);
--select * from lineup(2,40);
--select l.*, ISNULL(p.GP,0) GP,  ISNULL(p.D,0) D, 
--CASE WHEN l.Position = 'G' THEN CONCAT('(',p.DW,'-',p.DOT,'-',p.DSO,') ',p.DP) ELSE '' END DDesc,
----p.DP, p.DW, p.DOT, p.DSO, 
--ISNULL(p.I,0) I, 
--CASE WHEN l.Position = 'G' THEN CONCAT('(',p.IW,'-',p.IOT,'-',p.ISO,') ',p.IP) ELSE '' END IDesc,
----p.IP, p.IW, p.IOT, p.ISO, 
--ISNULL(p.T,0) T, 
--CASE WHEN l.Position = 'G' THEN CONCAT('(',p.TW,'-',p.TOT,'-',p.TSO,') ',p.TP) ELSE '' END TDesc
----p.TP, p.TW, p.TOT, p.TSO--, sp, sw, sot, sso
--from Lineup(2, 40) l
--left join (select ps.PlayerId,
--COUNT (gi.ID) GP,
--SUM (CASE WHEN CONVERT(date,gi.START_TIME)=CONVERT(date,GetDate()-1) THEN ISNULL (ps.P,0)*sc.P+ISNULL(gs.W,0)*sc.W+ISNULL(gs.OT,0)*sc.OT+ISNULL(gs.SO,0)*sc.SO ELSE 0 END) D, 
--SUM (CASE WHEN CONVERT(date,gi.START_TIME)=CONVERT(date,GetDate()-1) THEN (ps.P) ELSE 0 END) DP, 
--SUM (CASE WHEN CONVERT(date,gi.START_TIME)=CONVERT(date,GetDate()-1) THEN (gs.W) ELSE 0 END) DW, 
--SUM (CASE WHEN CONVERT(date,gi.START_TIME)=CONVERT(date,GetDate()-1) THEN (gs.OT) ELSE 0 END) DOT, 
--SUM (CASE WHEN CONVERT(date,gi.START_TIME)=CONVERT(date,GetDate()-1) THEN (gs.SO) ELSE 0 END) DSO, 
--SUM (CASE WHEN i.Id = 40 THEN ISNULL (ps.P,0)*sc.P+ISNULL(gs.W,0)*sc.W+ISNULL(gs.OT,0)*sc.OT+ISNULL(gs.SO,0)*sc.SO ELSE 0 END) I,
--SUM (CASE WHEN i.Id = 40 THEN ps.P ELSE 0 END) IP,
--SUM (CASE WHEN i.Id = 40 THEN gs.W ELSE 0 END) IW,
--SUM (CASE WHEN i.Id = 40 THEN gs.OT ELSE 0 END) IOT,
--SUM (CASE WHEN i.Id = 40 THEN gs.SO ELSE 0 END) ISO,
----(ISNULL(SUM (ps.P),0)*sc.P + ISNULL(SUM (gs.W),0)*sc.W + 
----ISNULL(SUM (gs.OT),0)*sc.OT + ISNULL(SUM (gs.SO),0)*sc.SO) T,
----ISNULL(SUM (ps.P),0) TP,
----ISNULL(SUM (gs.W),0) TW,
----ISNULL(SUM (gs.OT),0) TOT,
----ISNULL(SUM (gs.SO),0) TSO
--(SUM (ISNULL(ps.P,0))*sc.P + SUM (ISNULL(gs.W,0))*sc.W + 
--SUM (ISNULL(gs.OT,0))*sc.OT + SUM (ISNULL(gs.SO,0))*sc.SO) T,
--SUM (ISNULL(ps.P,0)) TP,
--SUM (ISNULL(gs.W,0)) TW,
--SUM (ISNULL(gs.OT,0)) TOT,
--SUM (ISNULL(gs.SO,0)) TSO
--from PLAYER_SUMMARY ps
--left join GOALIE_SUMMARY gs on ps.PlayerId = gs.PlayerId and ps.GAME_ID = gs.GAME_ID
--join GAME_INFO gi on ps.GAME_ID = gi.ID 
--join Interval i on Convert(date,gi.START_TIME) between i.StartDate and i.EndDate
--join PoolScoring sc on i.PoolId = sc.PoolId
--where i.SeasonId = (select SeasonId from Interval where Id = 40)
--GROUP BY ps.PlayerId, sc.P, sc.W, sc.OT, sc.SO) p on l.PlayerId = p.PlayerId
--FOR JSON AUTO;

select Convert(date,gi.START_TIME) GameDate, CASE WHEN p.NHLTeamCode = gi.HomeCode THEN gi.VisitorCode ELSE '@'+gi.HomeCode END Opponent
from Interval i 
join GAME_INFO gi on Convert(date,gi.START_TIME) between i.StartDate and i.EndDate
join NHLTeam t1 on t1.Code = gi.HomeCode
join NHLTeam t2 on t2.Code = gi.VisitorCode
join NHL_PLAYER p on p.NHLTeamCode IN (gi.HomeCode, gi.VisitorCode)
where p.ID = 8470594 and i.Id = 40

select * from Schedule(8470594,40);