#Lamb-Richmond Dataset Extension# 

LR Dataset Extension is a library that extends the Lamb-Richmond dataset using COOP station data from the US (NOAA/NWS) and Canada (when available).  The dataset provides a set of max/min temperature and precipitation from 50 uniform grid points over the US and Canada, east of the Rocky Mountains. This dataset is ideal for machine learning applications related to climate science.  The library extended the existing dataset from 2000 to 2010, including filling in some gaps in the 90s.

###Station Substitution###
Sometimes a COOP station will not have data for a particular day for a number of various reasons, or the measurement will have a flag that denotes it's likely an error.  In this case the library selects the nearest station (based on latitude and longitude) that has data for that day and uses it as a substitute station.

###Missing Canada Data###
Due to budget cuts there was missing Canada data at the time of extension, the data is said to exist on backup tape but needs to be made available online.