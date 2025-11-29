const categorizeAndSortTrips = (allTrips) => {
    const today = new Date();
    today.setHours(0, 0, 0, 0); // Normalize time to midnight

    const futureTrips = [];
    const archivedTrips = [];
    
    (allTrips || []).forEach(trip => {
        const tripEndDate = new Date(trip.endDate + 'T00:00:00');

        // Check for two conditions for archiving: 
        // 1. Explicitly marked as archived (isArchived property)
        // 2. The trip end date has passed (tripEndDate < today)
        if (trip.isArchived || tripEndDate < today) {
            archivedTrips.push(trip);
        } else {
            futureTrips.push(trip);
        }
    });

    // Sort future trips by start date (soonest first/ascending)
    futureTrips.sort((a, b) => 
        new Date(a.startDate + 'T00:00:00') - new Date(b.startDate + 'T00:00:00')
    );

    // Sort archived trips by end date (most recent archived first/descending)
    archivedTrips.sort((a, b) => 
        new Date(b.endDate + 'T00:00:00') - new Date(a.endDate + 'T00:00:00')
    );

    return { futureTrips, archivedTrips };
};

export default categorizeAndSortTrips;