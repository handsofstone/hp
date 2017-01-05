-- Season lineup vs max
select i.EndDate, a.Points Acutal, m.Points Max, a.Points*100/m.Points
from 
Interval i left join 
(select i.Id, SUM(l.Points) Points from Interval i cross apply LineupIntervalPoints(2,i.Id) l where i.SeasonId = 2 and l.Active = 1 group by i.Id) a on a.Id = i.Id
left join 
(select i.Id, SUM(m.Points) Points from Interval i cross apply MaxLineup(2,i.Id) m where i.SeasonId = 2 group by i.Id) m on m.Id = i.Id
where i.SeasonId = 2;

-- Totalled Season lineup vs max for all teams
select t.Name, SUM(a.Points) Acutal, SUM(m.Points) Max, SUM(a.Points)*100/SUM(m.Points) "Lineup Efficiency"
from 
Interval i cross join 
Team t left join
(select i.Id IntervalId, t.Id TeamId, SUM(l.Points) Points from Interval i cross join Team t cross apply LineupIntervalPoints(t.Id,i.Id) l where i.SeasonId = 2 and l.Active = 1 group by i.Id, t.Id) a on a.IntervalId = i.Id and a.TeamId = t.Id
left join 
(select i.Id IntervalId, t.Id TeamId, SUM(m.Points) Points from Interval i  cross join Team t cross apply MaxLineup(t.Id,i.Id) m where i.SeasonId = 2 group by i.Id, t.Id) m on m.IntervalId = i.Id and m.TeamId = t.Id
where i.SeasonId = 2
group by t.Id, t.Name;

-- missing lineups
select * from Team
Where NOT EXISTS
(SELECT * FROM LineupPlayer p join IntervaL i on p.IntervalId = i.Id
WHERE p.TeamId = Team.Id and i.PoolId = Team.PoolId and 
p.IntervalId IN (select Id from Interval where getdate() between StartDate and EndDate))
AND EXISTS (SELECT 1 from Interval where getdate() between StartDate and EndDate and PoolId = Team.PoolId );

select 8, 46, PlayerId, Active, Position from Lineup(8,46)

select * from Team t join Interval i on t.PoolId = i.PoolId
where NOT EXISTS (SELECT * FROM LineupPlayer lp WHERE lp.TeamId = t.Id and lp.IntervalId = i.Id)
AND i.Id IN(select Id from Interval where getdate() between StartDate and EndDate);

Select Count(*) from LineupPlayer;