select 'player' 'Type', np.ID, CONCAT (np.LAST_NAME, ', ',np.FIRST_NAME) 'Description'
from RosterPlayer rp join NHL_PLAYER np on rp.PlayerId = np.ID
where rp.TeamId = 2
union 
select 'pick' 'Type', Id, Pick 'Description' from OwnedPicks(2)
order by Description;