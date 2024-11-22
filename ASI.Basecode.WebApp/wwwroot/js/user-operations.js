//pagination
let currentPage = 1;
const rowsPerPage = 7;
let totalPages = 1;

function setupPagination(totalItems) {
    const totalPages = Math.ceil(totalItems / rowsPerPage);
    const paginationContainer = document.querySelector('.custom-pagination-btn');

    if (paginationContainer) {
        let html = `
            <button class="custom-pagination-btn" onclick="goToPage(1)" ${currentPage === 1 ? 'disabled' : ''}>
                <i class="fa-solid fa-angles-left"></i>
            </button>
            <button "custom-pagination-btn" onclick="goToPage(${currentPage - 1})" ${currentPage === 1 ? 'disabled' : ''}>
                <i class="fa-solid fa-angle-left"></i>
            </button>
        `;

        if (totalPages <= 3) {
            // Show all pages if total pages are 3 or fewer
            for (let i = 1; i <= totalPages; i++) {
                html += `<a href="#" onclick="goToPage(${i})" class="${currentPage === i ? 'active' : ''}">${i}</a>`;
            }
        } else {
            if (currentPage === 1) {
                // Page 1: Show 1 2 ... n
                html += `<a href="#" onclick="goToPage(1)" class="active">1</a>`;
                html += `<a href="#" onclick="goToPage(2)">2</a>`;
                html += `<span class="ellipsis">...</span>`;
                html += `<a href="#" onclick="goToPage(${totalPages})">${totalPages}</a>`;
            } else if (currentPage === 2) {
                // Page 2: Show 2 3 4 (No ellipsis)
                html += `<a href="#" onclick="goToPage(2)" class="active">2</a>`;
                html += `<a href="#" onclick="goToPage(3)">3</a>`;
                html += `<a href="#" onclick="goToPage(4)">4</a>`;
            } else if (currentPage === 3) {
                // Page 3: Show ... 3 4 (No ellipsis, Next/Last disabled)
                html += `<span class="ellipsis">...</span>`;
                html += `<a href="#" onclick="goToPage(3)" class="active">3</a>`;
                html += `<a href="#" onclick="goToPage(4)">4</a>`;
            } else if (currentPage === totalPages) {
                // Last Page (Page 4): Show ... 3 4 (Next/Last buttons disabled)
                html += `<span class="ellipsis">...</span>`;
                html += `<a href="#" onclick="goToPage(3)">3</a>`;
                html += `<a href="#" onclick="goToPage(4)" class="active">4</a>`;
            } else {
                // Middle Pages: Show ... currentPage-1 currentPage currentPage+1 ...
                if (currentPage > 2) {
                    html += `<a href="#" onclick="goToPage(1)">1</a>`;
                    html += `<span class="ellipsis">...</span>`;
                }
                html += `<a href="#" onclick="goToPage(${currentPage - 1})">${currentPage - 1}</a>`;
                html += `<a href="#" onclick="goToPage(${currentPage})" class="active">${currentPage}</a>`;
                html += `<a href="#" onclick="goToPage(${currentPage + 1})">${currentPage + 1}</a>`;
                if (currentPage < totalPages - 1) {
                    html += `<span class="ellipsis">...</span>`;
                    html += `<a href="#" onclick="goToPage(${totalPages})">${totalPages}</a>`;
                }
            }
        }
        
        // Next and Last buttons
        html += `
            <button onclick="goToPage(${currentPage + 1})" ${currentPage === totalPages ? 'disabled' : ''}>
                <i class="fa-solid fa-angle-right"></i>
            </button>
            <button onclick="goToPage(${totalPages})" ${currentPage === totalPages ? 'disabled' : ''}>
                <i class="fa-solid fa-angles-right"></i>
            </button>
        `;

        paginationContainer.innerHTML = html;
    }

    const resultsInfo = document.querySelector('.results-info');
    if (resultsInfo) {
        const startItem = (currentPage - 1) * rowsPerPage + 1;
        const endItem = Math.min(currentPage * rowsPerPage, totalItems);
        resultsInfo.textContent = `Showing ${startItem} to ${endItem} out of ${totalItems} results`;
    }
}

function goToPage(page) {
    const totalPages = Math.ceil(document.querySelectorAll('.info-table table tbody tr:not(.empty-row)').length / rowsPerPage);

    if (page < 1 || page > totalPages) return; 
    currentPage = page; 
    displayUserTableRows(); 
    setupPagination(totalItems); 
}

function displayUserTableRows() {
    const table = document.querySelector('.info-table table tbody');
    const rows = table.getElementsByTagName('tr');
    const totalItems = Array.from(rows).filter(row => !row.classList.contains('empty-row')).length;

    const startIndex = (currentPage - 1) * rowsPerPage;
    const endIndex = Math.min(startIndex + rowsPerPage, totalItems);

    for (let i = 0; i < rows.length; i++) {
        rows[i].style.display = 'none';
    }

    let displayedRows = 0;
    for (let i = 0; i < rows.length; i++) {
        if (!rows[i].classList.contains('empty-row')) {
            if (displayedRows >= startIndex && displayedRows < endIndex) {
                rows[i].style.display = '';
            }
            displayedRows++;
        }
    }

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

document.addEventListener('DOMContentLoaded', function() {
    const totalItems = document.querySelectorAll('.info-room-add-container .info-table table tbody tr').length; 
    setupPagination(totalItems);
    displayUserTableRows(); 
});