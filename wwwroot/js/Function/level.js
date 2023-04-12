var dataTable;
$(document).ready(function () {
    dataTable = $('#tbLevel').DataTable({
        "ajax": {
            "url": "/Level/GetAllLevel",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            {
                "data": "levelCode",
                "width": "20%"
            },
            {
                "data": "levelName",
                "width": "20%"
            },
            {
                "data": "unitPrice",
                "width": "20%"
            },
            {
                "data": "status",
                "width": "20%"
            },
            {
                render: function (data, type, full, meta) {
                    return "<button onclick='ProductEdit(" + full.id + ")' class='btn btn-warning btn-xs' style='margin-right:5px;'>Edit</button>" + "<button onclick='ProductDelete(" + full.id + ")' class='btn btn-danger btn-xs'>Delete</button>";
                }
            }
        ],
        "language": {
            "emptyTable": "Not Found Data"
        },
        "width": "100%"

    });
});