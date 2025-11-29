// --- Date Formatting Helper (Re-used for consistency) ---
const formatDate = (dateString) => {
    if (!dateString) return 'N/A';
    try {
        return new Date(dateString + 'T00:00:00').toLocaleDateString('en-US', {
            month: 'short', 
            day: 'numeric', 
            year: 'numeric'
        });
    } catch {
        return dateString;
    }
};

export default formatDate;