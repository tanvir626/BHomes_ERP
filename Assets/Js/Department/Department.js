
    $(document).ready(function () {

        ShowTable("HR_Department");

    // InsertPage
    $("#pageForm").on("submit", function (e) {
        e.preventDefault(); // prevent normal form submit
    $("#loadingSpinner").show(); // show loader

    $.ajax({
        url: '@Url.Action("Create_Department", "HRConfig")', // Controller Action
    type: "POST",
    data: $(this).serialize(), // send form data
    success: function (response) {
                     if (response.info) {
        toastr.success("Successfully Done");
    $("#loadingSpinner").hide(); // hide loader
    $('#exampleModal').modal('hide'); // close modal
    window.location.reload();
                     } else {
        $('#exampleModal').modal('hide');
    $("#loadingSpinner").hide();
    toastr.error("Same Data Exists");
                     }
                 },
    error: function (xhr) {
        $("#loadingSpinner").hide();
    alert("Error occurred: " + xhr.responseText);
                 }
             });
         });

    // UpdatePage
    $('#EditDepartmentForm').on("submit", function (e) {
        e.preventDefault(); // prevent page reload
    var form = $(this);

    $.ajax({
        url: form.attr('action'), // UpdatePage action
    type: 'POST',
    data: form.serialize(),   // send all form data
    success: function (response) {
                     if (response.info) {
        $('#EditModal').modal('hide');
    toastr.success("Page updated successfully!");
    window.location.reload();
                     } else {
        $('#EditModal').modal('hide');
    toastr.error(response.message || "Something went wrong!");
                     }
                 },
    error: function (xhr) {
        toastr.error("Error occurred: " + xhr.responseText);
                 }
             });
         });

     });


    //ShowTable
    function ShowTable(TableName) {
        $.ajax({
            url: '/api/ShowTableApi/',
            type: "GET",
            data: { TableName: TableName },
            success: function (response) {
                $("#loadingSpinner").hide();

                var tbody = $("#deptTableBody");
                tbody.empty(); // clear old rows

                if (response.row && response.row.length > 0) {
                    var sl = 1;
                    $.each(response.row, function (i, d) {
                        tbody.append(`
                             <tr>
                                 <td class="text-center">${sl}</td>
                                 <td class="text-center">${d.HR_DeptCode}</td>
                                 <td class="text-center">${d.HR_DeptName}</td>
                                 <td class="text-center">${d.Status}</td>
                                 <td class="text-center">
                                     <button class="btn btn-sm btn-primary" onclick="ShowindividualRow('HR_Department','HR_DeptCode',${d.HR_DeptCode})">Edit</button>
                                     <button class="btn btn-sm btn-danger" onclick="Delete('HR_Department','HR_DeptCode',${d.HR_DeptCode})">Delete</button>
                                 </td>
                             </tr>
                         `);
                        sl++;
                    });
                } else {
                    tbody.append(`<tr><td colspan="5" class="text-center">No data found</td></tr>`);
                }
            },
            error: function (xhr) {
                $("#loadingSpinner").hide();
                alert("Error occurred: " + xhr.responseText);
            }
        });
     }


    // ShowIndividualRow
    function ShowindividualRow(TableName, ColName, id) {
         if (!confirm("Are you sure you want to edit this row?"))
    return;

    $("#loadingSpinner").show(); // show loader

    $.ajax({
        url: '/api/ShowIndivisualRowApi/',
    type: "GET",
    data: {TableName: TableName, ColName: ColName, id: id },
    success: function (response) {
        $("#loadingSpinner").hide();

                 if (response.row && response.row.length > 0) {
                     var row = response.row[0];
    $('#EditModal').modal('show');
    $('#EditDeptName').val(row.HR_DeptName);
    $('#EditDeptStatus').prop('checked', row.Status);
                     
                 } else {
        alert("No data found!");
                 }
             },
    error: function (xhr) {
        $("#loadingSpinner").hide();
    alert("Error occurred: " + xhr.responseText);
             }
         });
     }

    // Delete Row
    function Delete(TableName, ColName, id) {
         if (!confirm("Are you sure you want to delete this row?"))
    return;

    $("#loadingSpinner").show();

    $.ajax({
        url: '/Api/DeleteApi/',
    type: "POST",
    data: {TableName: TableName, ColName: ColName, id: id },
    success: function (response) {
        $("#loadingSpinner").hide();
    if (response.info) {
        toastr.success("Successfully Deleted");
                     
                 } else {
        toastr.error("Something Went Wrong");
                 }
    window.location.reload();
             },
    error: function (xhr) {
        $("#loadingSpinner").hide();
    alert("Error occurred: " + xhr.responseText);
             }
         });
     }