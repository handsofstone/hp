 
select a.Id, coalesce(p.LAST_NAME+', '+p.FIRST_NAME, s.Name + ' ' + t.Name + ' ' + CAST(d.Round AS VARCHAR(10))
        +
        CASE
            WHEN d.Round % 100 IN (11,12,13) THEN 'th' --first checks for exception
            WHEN d.Round % 10 = 1 THEN 'st'
            WHEN d.Round % 10 = 2 THEN 'nd'
            WHEN d.Round % 10 = 3 THEN 'rd'
            ELSE 'th' --works for num % 10 IN (4,5,6,7,8,9,0)
        END
) AssetName from nlpool.TeamAsset a
left join nlPool.RosterPlayer r on (a.AssetType = 'roster' and a.AssetId = r.Id)
left join nlPool.DraftPick d on (a.AssetType = 'pick' and a.AssetId = d.Id)
left join dbo.NHL_PLAYER p on r.PlayerId = p.ID
left join nlpool.Season s on d.SeasonId = s.Id
left join nlpool.Team t on d.TeamId = t.Id
where a.TeamId = 5
order by 
	CASE WHEN r.ID IS NULL THEN '1' ELSE '0' END,
	CASE 
	WHEN r.Position = 'C' THEN '1'
	WHEN r.Position = 'R' THEN '2'
	WHEN r.Position = 'L' THEN '3'
	WHEN r.Position = 'D' THEN '4'
	WHEN r.Position = 'G' THEN '5' END, p.LAST_NAME, p.FIRST_NAME, s.Name, d.Round, t.Name;
