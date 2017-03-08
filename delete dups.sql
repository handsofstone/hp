
delete from LineupPlayer where Id in
(select Id
from 
(SELECT Id, PlayerId, IntervalId, TeamId, Active, RN = ROW_NUMBER() OVER (PARTITION BY PlayerId, IntervalId, TeamId ORDER BY active, Id desc)
FROM LineupPlayer) t
where rn > 1);
