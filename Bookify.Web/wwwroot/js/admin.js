/* ============================================
   BOOKIFY - ADMIN JAVASCRIPT
   Admin-specific JavaScript utilities
   ============================================ */

// Import main utilities
// (In a real build system, you'd import from main.js)

// ========== Admin Sidebar Toggle ==========
function initAdminSidebar() {
    const sidebarToggle = document.querySelector('.admin-sidebar-toggle');
    const sidebar = document.querySelector('.admin-sidebar');
    const content = document.querySelector('.admin-content');
    
    if (sidebarToggle && sidebar) {
        sidebarToggle.addEventListener('click', function() {
            sidebar.classList.toggle('collapsed');
            if (content) {
                content.classList.toggle('sidebar-collapsed');
            }
        });
    }
}

// ========== Admin Table Actions ==========
function initAdminTableActions() {
    const deleteButtons = document.querySelectorAll('.admin-action-btn-delete');
    
    deleteButtons.forEach(button => {
        button.addEventListener('click', function(e) {
            if (!confirm('Are you sure you want to delete this item?')) {
                e.preventDefault();
                return false;
            }
        });
    });
}

// ========== Admin Form Auto-save (Optional) ==========
function initAutoSave() {
    const forms = document.querySelectorAll('form[data-autosave]');
    
    forms.forEach(form => {
        const inputs = form.querySelectorAll('input, textarea, select');
        const formId = form.dataset.autosave;
        
        // Load saved data
        const savedData = localStorage.getItem(`autosave_${formId}`);
        if (savedData) {
            try {
                const data = JSON.parse(savedData);
                Object.keys(data).forEach(key => {
                    const input = form.querySelector(`[name="${key}"]`);
                    if (input && input.type !== 'password') {
                        input.value = data[key];
                    }
                });
            } catch (e) {
                console.error('Error loading autosave data:', e);
            }
        }
        
        // Save on input change
        inputs.forEach(input => {
            input.addEventListener('input', function() {
                const formData = new FormData(form);
                const data = {};
                formData.forEach((value, key) => {
                    if (key !== '__RequestVerificationToken') {
                        data[key] = value;
                    }
                });
                localStorage.setItem(`autosave_${formId}`, JSON.stringify(data));
            });
        });
        
        // Clear on successful submit
        form.addEventListener('submit', function() {
            localStorage.removeItem(`autosave_${formId}`);
        });
    });
}

// ========== Admin Search/Filter ==========
function initAdminSearch() {
    const searchInputs = document.querySelectorAll('.admin-search-input');
    
    searchInputs.forEach(input => {
        input.addEventListener('input', function() {
            const searchTerm = this.value.toLowerCase();
            const table = this.closest('.admin-table-wrapper')?.querySelector('.admin-table');
            
            if (table) {
                const rows = table.querySelectorAll('tbody tr');
                
                rows.forEach(row => {
                    const text = row.textContent.toLowerCase();
                    row.style.display = text.includes(searchTerm) ? '' : 'none';
                });
            }
        });
    });
}

// ========== Admin Bulk Actions ==========
function initBulkActions() {
    const selectAllCheckbox = document.querySelector('.select-all-checkbox');
    const itemCheckboxes = document.querySelectorAll('.item-checkbox');
    const bulkActionForm = document.querySelector('.bulk-action-form');
    const bulkActionSelect = document.querySelector('.bulk-action-select');
    
    if (selectAllCheckbox && itemCheckboxes.length > 0) {
        selectAllCheckbox.addEventListener('change', function() {
            itemCheckboxes.forEach(checkbox => {
                checkbox.checked = this.checked;
            });
            updateBulkActionButton();
        });
        
        itemCheckboxes.forEach(checkbox => {
            checkbox.addEventListener('change', function() {
                updateBulkActionButton();
                updateSelectAllState();
            });
        });
    }
    
    function updateSelectAllState() {
        if (selectAllCheckbox) {
            const checkedCount = Array.from(itemCheckboxes).filter(cb => cb.checked).length;
            selectAllCheckbox.checked = checkedCount === itemCheckboxes.length && itemCheckboxes.length > 0;
            selectAllCheckbox.indeterminate = checkedCount > 0 && checkedCount < itemCheckboxes.length;
        }
    }
    
    function updateBulkActionButton() {
        const checkedCount = Array.from(itemCheckboxes).filter(cb => cb.checked).length;
        const bulkActionButton = document.querySelector('.bulk-action-button');
        
        if (bulkActionButton) {
            bulkActionButton.disabled = checkedCount === 0;
            bulkActionButton.textContent = checkedCount > 0 
                ? `Apply to ${checkedCount} item(s)` 
                : 'Apply Action';
        }
    }
    
    if (bulkActionForm && bulkActionSelect) {
        bulkActionForm.addEventListener('submit', function(e) {
            const checkedItems = Array.from(itemCheckboxes).filter(cb => cb.checked);
            
            if (checkedItems.length === 0) {
                e.preventDefault();
                alert('Please select at least one item.');
                return false;
            }
            
            if (!confirm(`Are you sure you want to ${bulkActionSelect.value} ${checkedItems.length} item(s)?`)) {
                e.preventDefault();
                return false;
            }
        });
    }
}

// ========== Admin Statistics Refresh ==========
function refreshAdminStats() {
    const refreshButton = document.querySelector('.refresh-stats-btn');
    
    if (refreshButton) {
        refreshButton.addEventListener('click', function() {
            this.disabled = true;
            this.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Refreshing...';
            
            // Simulate API call (replace with actual API call)
            setTimeout(() => {
                location.reload();
            }, 1000);
        });
    }
}

// ========== Admin Export Data ==========
function initDataExport() {
    const exportButtons = document.querySelectorAll('.export-data-btn');
    
    exportButtons.forEach(button => {
        button.addEventListener('click', function() {
            const format = this.dataset.format || 'csv';
            const table = this.closest('.admin-table-wrapper')?.querySelector('.admin-table');
            
            if (table) {
                exportTableToFormat(table, format);
            }
        });
    });
}

function exportTableToFormat(table, format) {
    const rows = table.querySelectorAll('tr');
    let data = [];
    
    rows.forEach(row => {
        const rowData = [];
        row.querySelectorAll('th, td').forEach(cell => {
            rowData.push(cell.textContent.trim());
        });
        data.push(rowData);
    });
    
    if (format === 'csv') {
        const csv = data.map(row => row.join(',')).join('\n');
        downloadFile(csv, 'data.csv', 'text/csv');
    } else if (format === 'json') {
        const headers = data[0];
        const json = data.slice(1).map(row => {
            const obj = {};
            headers.forEach((header, i) => {
                obj[header] = row[i];
            });
            return obj;
        });
        downloadFile(JSON.stringify(json, null, 2), 'data.json', 'application/json');
    }
}

function downloadFile(content, filename, contentType) {
    const blob = new Blob([content], { type: contentType });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = filename;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    URL.revokeObjectURL(url);
}

// ========== Initialize on DOM Load ==========
document.addEventListener('DOMContentLoaded', function() {
    initAdminSidebar();
    initAdminTableActions();
    initAutoSave();
    initAdminSearch();
    initBulkActions();
    refreshAdminStats();
    initDataExport();
});

// ========== Export Functions ==========
window.BookifyAdmin = {
    initAdminSidebar,
    initAdminTableActions,
    initAdminSearch,
    initBulkActions,
    exportTableToFormat
};

