function addRoom(event) {
    event.preventDefault();
    const roomData = {
        roomname: document.getElementById('roomName').value,
        maxCapacity: document.getElementById('capacity').value
    };

    $.ajax({
        url: '/Account/NewRoom',
        type: 'POST',
        data: roomData,
        success: function(response) {
            if (response.success) {
                console.log('Room added successfully:', response);
                alert('Room added successfully!');
                closeModal();
                location.reload();
            } else {
                console.error('Error adding room:', response.message);
                alert(response.message || 'Failed to add room');
            }
        },
        error: function(error) {
            console.error('Error adding room:', error);
            alert('Failed to add room. Please check console for details.');
        }
    });
}

function deleteRoom(roomId) {
    if (confirm('Are you sure you want to delete this room?')) {
        $.ajax({
            url: '/Home/DeleteRoom',
            type: 'POST',
            data: { roomId: roomId },
            success: function(response) {
                console.log('Room deleted successfully:', response);
                alert('Room deleted successfully!');
                location.reload();
            },
            error: function(error) {
                console.error('Error deleting room:', error);
                alert('Failed to delete room. Please check console for details.');
            }
        });
    }
}

function editRoom(roomId) {
    // First fetch the room details
    $.ajax({
        url: `/Home/GetRoom/${roomId}`,
        type: 'GET',
        success: function(room) {
            // Populate the modal with room details
            document.getElementById('roomName').value = room.roomname;
            document.getElementById('capacity').value = room.maxCapacity;
            
            // Store the room ID for the update operation
            document.getElementById('addUserForm').dataset.roomId = roomId;
            
            // Open the modal
            openModal();
        },
        error: function(error) {
            console.error('Error fetching room details:', error);
            alert('Failed to fetch room details. Please try again.');
        }
    });
}

function showBookings(roomId) {
    $.ajax({
        url: `/Account/GetBookingsbyRoomid?roomid=${roomId}`,
        type: 'POST',
        success: function(bookings) {
            if (Array.isArray(bookings)) {
                populateBookingsTable(bookings);
                openBookingsModal();
            } else {
                console.error('Invalid response format:', bookings);
                alert('Failed to fetch bookings');
            }
        },
        error: function(error) {
            console.error('Error fetching bookings:', error);
            alert('Failed to fetch bookings');
        }
    });
}

function populateBookingsTable(bookings) {
    const tbody = document.getElementById('bookingsTableBody');
    tbody.innerHTML = '';

    if (bookings.length === 0) {
        const row = document.createElement('tr');
        row.innerHTML = '<td colspan="6" style="text-align: center;">No bookings found</td>';
        tbody.appendChild(row);
        return;
    }

    bookings.forEach(booking => {
        // Only show bookings with status 'RESERVED' or 'VACANT'
        if (booking.status === 'RESERVED' || booking.status === 'VACANT') {
            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${booking.customer?.custfname || '--'}</td>
                <td>${booking.customer?.contact || '--'}</td>
                <td>${booking.bookingDate ? new Date(booking.bookingDate).toLocaleDateString() : '--'}</td>
                <td>${booking.timeIn ? new Date('1970-01-01T' + booking.timeIn).toLocaleTimeString([], {hour: '2-digit', minute:'2-digit'}) : '--'}</td>
                <td>${booking.timeOut ? new Date('1970-01-01T' + booking.timeOut).toLocaleTimeString([], {hour: '2-digit', minute:'2-digit'}) : '--'}</td>
                <td>${booking.status}</td>
            `;
            tbody.appendChild(row);
        }
    });
}

function openBookingsModal() {
    const modal = document.getElementById('bookingsModal');
    if (modal) {
        modal.style.display = 'block';
    } else {
        console.error('Bookings modal not found');
    }
}

function closeBookingsModal() {
    const modal = document.getElementById('bookingsModal');
    if (modal) {
        modal.style.display = 'none';
    }
}

let allBookings = []; // Store all bookings
let currentSortColumn = null;
let isAscending = true;

function showAllBookings() {
    $.ajax({
        url: '/Account/GetAllBookings',
        type: 'GET',
        success: function(bookings) {
            if (Array.isArray(bookings)) {
                allBookings = bookings;
                populateAllBookingsTable(bookings);
                openAllBookingsModal();
                initializeSortingListeners();
            } else {
                console.error('Invalid response format:', bookings);
                alert('Failed to fetch bookings');
            }
        },
        error: function(error) {
            console.error('Error fetching all bookings:', error);
            alert('Failed to fetch bookings');
        }
    });
}

function populateAllBookingsTable(bookings) {
    const tbody = document.getElementById('allBookingsTableBody');
    if (!tbody) return;
    
    tbody.innerHTML = '';

    if (!bookings || bookings.length === 0) {
        tbody.innerHTML = '<tr><td colspan="8" style="text-align: center;">No bookings found</td></tr>';
        return;
    }

    bookings.forEach(booking => {
        const statusClass = booking.status ? booking.status.toLowerCase() : '';
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${booking.room?.roomNumber || '--'}</td>
            <td>${booking.room?.roomname || '--'}</td>
            <td>${booking.customer?.custfname || '--'}</td>
            <td>${booking.customer?.contact || '--'}</td>
            <td>${booking.bookingDate ? new Date(booking.bookingDate).toLocaleDateString() : '--'}</td>
            <td>${booking.timeIn ? new Date('1970-01-01T' + booking.timeIn).toLocaleTimeString([], {hour: '2-digit', minute:'2-digit'}) : '--'}</td>
            <td>${booking.timeOut ? new Date('1970-01-01T' + booking.timeOut).toLocaleTimeString([], {hour: '2-digit', minute:'2-digit'}) : '--'}</td>
            <td class="status-cell status-${statusClass}">${booking.status}</td>
        `;
        tbody.appendChild(row);
    });
}

function initializeSortingListeners() {
    const headers = document.querySelectorAll('.sortable-header');
    headers.forEach(header => {
        header.addEventListener('click', () => {
            const sortKey = header.dataset.sort;
            
            // Toggle sort direction if clicking the same column
            if (currentSortColumn === sortKey) {
                isAscending = !isAscending;
            } else {
                currentSortColumn = sortKey;
                isAscending = true;
            }

            // Sort the bookings
            sortBookings(sortKey);
            
            // Update sorting indicators
            headers.forEach(h => h.classList.remove('sort-asc', 'sort-desc'));
            header.classList.add(isAscending ? 'sort-asc' : 'sort-desc');
        });
    });
}

function sortBookings(sortKey) {
    const sortedBookings = [...allBookings].sort((a, b) => {
        let valueA, valueB;

        switch(sortKey) {
            case 'roomNumber':
                valueA = a.room?.roomNumber || 0;
                valueB = b.room?.roomNumber || 0;
                break;
            case 'roomName':
                valueA = (a.room?.roomname || '').toLowerCase();
                valueB = (b.room?.roomname || '').toLowerCase();
                break;
            case 'guestName':
                valueA = (a.customer?.custfname || '').toLowerCase();
                valueB = (b.customer?.custfname || '').toLowerCase();
                break;
            case 'bookingDate':
                valueA = new Date(a.bookingDate || 0).getTime();
                valueB = new Date(b.bookingDate || 0).getTime();
                break;
            case 'timeIn':
                valueA = a.timeIn || '';
                valueB = b.timeIn || '';
                break;
            case 'timeOut':
                valueA = a.timeOut || '';
                valueB = b.timeOut || '';
                break;
            case 'status':
                valueA = a.status || '';
                valueB = b.status || '';
                break;
            default:
                return 0;
        }

        // Compare values based on sort direction
        if (valueA < valueB) return isAscending ? -1 : 1;
        if (valueA > valueB) return isAscending ? 1 : -1;
        return 0;
    });

    populateAllBookingsTable(sortedBookings);
}

function openAllBookingsModal() {
    const modal = document.getElementById('allBookingsModal');
    if (modal) {
        modal.style.display = 'block';
    }
}

function closeAllBookingsModal() {
    const modal = document.getElementById('allBookingsModal');
    if (modal) {
        modal.style.display = 'none';
    }
}

// Update the window click handler to handle both modals
window.onclick = function(event) {
    const bookingsModal = document.getElementById('bookingsModal');
    const allBookingsModal = document.getElementById('allBookingsModal');
    
    if (event.target === bookingsModal) {
        closeBookingsModal();
    }
    if (event.target === allBookingsModal) {
        closeAllBookingsModal();
    }
}

function filterRoomTable() {
    const searchInput = document.getElementById('roomSearchInput');
    const filter = searchInput.value.toLowerCase().trim();
    const table = document.querySelector('.room-table table');
    const rows = table.getElementsByTagName('tr');
    let visibleCount = 0;

    // Skip header row
    for (let i = 1; i < rows.length; i++) {
        const row = rows[i];
        const cells = row.getElementsByTagName('td');
        let shouldShow = false;

        if (filter === '') {
            // Show all rows if search is empty
            shouldShow = true;
        } else {
            // Search through each cell in the row
            for (let j = 0; j < cells.length; j++) {
                const cell = cells[j];
                if (cell) {
                    const text = cell.textContent || cell.innerText;
                    // Check if cell text contains search term
                    if (text.toLowerCase().indexOf(filter) > -1) {
                        shouldShow = true;
                        break;
                    }
                }
            }
        }

        // Show/hide the row based on search match
        if (shouldShow) {
            row.style.display = '';
            visibleCount++;
        } else {
            row.style.display = 'none';
        }
    }

    // Show/hide no results message
    const noResultsMessage = document.querySelector('.no-results-message');
    if (noResultsMessage) {
        if (visibleCount === 0 && filter !== '') {
            noResultsMessage.style.display = 'block';
            noResultsMessage.textContent = `No rooms found matching "${filter}"`;
        } else {
            noResultsMessage.style.display = 'none';
        }
    }

    // Log search results for debugging
    console.log(`Search: "${filter}" - Found ${visibleCount} matches`);
}

// Add debounce function to improve performance
function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// Replace the original filterRoomTable with debounced version
const debouncedFilterRoomTable = debounce(filterRoomTable, 300);

// Update the search input event listener
document.addEventListener('DOMContentLoaded', function() {
    const searchInput = document.getElementById('roomSearchInput');
    if (searchInput) {
        searchInput.addEventListener('keyup', debouncedFilterRoomTable);
        searchInput.addEventListener('search', debouncedFilterRoomTable); // For clear button
    }
});

// Add these pagination variables
let currentPage = 1;
const rowsPerPage = 7;
let totalPages = 1;

function setupPagination(totalItems) {
    totalPages = Math.ceil(totalItems / rowsPerPage);
    
    // Update pagination UI
    const paginationContainer = document.querySelector('.custom-pagination-btn');
    if (paginationContainer) {
        let html = `
            <button onclick="goToPage(1)" ${currentPage === 1 ? 'disabled' : ''}>
                <i class="fa-solid fa-angles-left"></i>
            </button>
            <button onclick="goToPage(${currentPage - 1})" ${currentPage === 1 ? 'disabled' : ''}>
                <i class="fa-solid fa-angle-left"></i>
            </button>
        `;

        // Add page numbers with limits
        if (totalPages <= 3) {
            // Show all pages if total pages are 3 or less
            for (let i = 1; i <= totalPages; i++) {
                html += `<a href="#" onclick="goToPage(${i})" class="${currentPage === i ? 'active' : ''}">${i}</a>`;
            }
        } else {
            // Show first page
            html += `<a href="#" onclick="goToPage(1)" class="${currentPage === 1 ? 'active' : ''}">1</a>`;
            // Show second page
            if (currentPage > 1) {
                html += `<a href="#" onclick="goToPage(2)" class="${currentPage === 2 ? 'active' : ''}">2</a>`;
            }
            // Show ellipsis if current page is greater than 2
            if (currentPage > 2) {
                html += `<span>...</span>`;
            }
            // Show last page if current page is not the last
            if (currentPage < totalPages) {
                html += `<a href="#" onclick="goToPage(${totalPages})" class="${currentPage === totalPages ? 'active' : ''}">${totalPages}</a>`;
            }
        }

        html += `
            <button onclick="goToPage(${currentPage + 1})" ${currentPage === totalPages ? 'disabled' : ''}>
                <i class="fa-solid fa-angle-right"></i>
            </button>
            <button onclick="goToPage(totalPages)" ${currentPage === totalPages ? 'disabled' : ''}>
                <i class="fa-solid fa-angles-right"></i>
            </button>
        `;

        paginationContainer.innerHTML = html;
    }

    // Update results info
    const resultsInfo = document.querySelector('.results-info');
    if (resultsInfo) {
        const startItem = (currentPage - 1) * rowsPerPage + 1;
        const endItem = Math.min(currentPage * rowsPerPage, totalItems);
        resultsInfo.textContent = `Showing ${startItem} to ${endItem} out of ${totalItems} results`;
    }
}

function goToPage(page) {
    if (page < 1 || page > totalPages) return;
    currentPage = page;
    displayRoomTableRows();
}

function displayRoomTableRows() {
    const table = document.querySelector('.room-table table tbody');
    const rows = table.getElementsByTagName('tr');
    const totalItems = Array.from(rows).filter(row => !row.classList.contains('empty-row')).length; // Count only non-empty rows
    
    // Calculate start and end indices for current page
    const startIndex = (currentPage - 1) * rowsPerPage;
    const endIndex = Math.min(startIndex + rowsPerPage, totalItems);
    
    // Hide all rows first
    for (let i = 0; i < rows.length; i++) {
        rows[i].style.display = 'none';
    }
    
    // Show rows for current page
    let displayedRows = 0; // To count how many actual rows are displayed
    for (let i = 0; i < rows.length; i++) {
        if (!rows[i].classList.contains('empty-row')) {
            if (displayedRows >= startIndex && displayedRows < endIndex) {
                rows[i].style.display = ''; // Show the row
            }
            displayedRows++;
        }
    }

    // Add empty rows if needed
    const existingEmptyRows = table.querySelectorAll('.empty-row');
    existingEmptyRows.forEach(row => row.remove());

    const visibleRows = endIndex - startIndex;
    if (visibleRows < rowsPerPage) {
        const emptyRowsNeeded = rowsPerPage - visibleRows;
        for (let i = 0; i < emptyRowsNeeded; i++) {
            const emptyRow = document.createElement('tr');
            emptyRow.className = 'empty-row';
            emptyRow.innerHTML = `
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            `;
            table.appendChild(emptyRow);
        }
    }

    setupPagination(totalItems);
}

// Update the existing room table population logic
function populateRoomTable(rooms) {
    const tbody = document.querySelector('.room-table table tbody');
    tbody.innerHTML = '';

    rooms.forEach(room => {
        const row = document.createElement('tr');
        // Your existing row population code
    });

    // Initialize pagination
    currentPage = 1;
    displayRoomTableRows();
}

document.addEventListener('DOMContentLoaded', function() {
    // Your existing DOMContentLoaded code
    
    // Initialize pagination
    displayRoomTableRows();
});

// Update your add room success handler
function handleAddRoomSuccess() {
    // Your existing success handling code
    
    // Reset to first page and refresh table
    currentPage = 1;
    displayRoomTableRows();
}