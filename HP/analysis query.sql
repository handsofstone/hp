select ps.PlayerId,
SUM ( TOP (r.Size) ISNULL (ps.P,0)*sc.P+ISNULL(gs.W,0)*sc.W+ISNULL(gs.OT,0)*sc.OT+ISNULL(gs.SO,0)*sc.SO) I,
SUM ( ps.P) IP,
SUM ( gs.W) IW,
SUM ( gs.OT) IOT,
SUM ( gs.SO) ISO,
r.Size,
r.Position
from PLAYER_SUMMARY ps
left join GOALIE_SUMMARY gs on ps.PlayerId = gs.PlayerId and ps.GAME_ID = gs.GAME_ID
join GAME_INFO gi on ps.GAME_ID = gi.ID 
join Interval i on Convert(date,gi.START_TIME) between i.StartDate and i.EndDate
join LineupPlayer l on i.Id = l.IntervalId and ps.PlayerId = l.PlayerId
join PoolScoring sc on i.PoolId = sc.PoolId
join PoolLineupRule r on i.PoolId = r.PoolId and l.Position = r.Position
where i.Id = 40 and l.TeamId = 2
group by ps.PlayerId, r.Size, r.Position;																																								