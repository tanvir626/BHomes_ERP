$(document).on("submit", ".ajax-form", function (e) {
    e.preventDefault();

    var form = $(this);
    var btn = form.find('button[type=submit]');
    btn.prop("disabled", true); // disable submit button temporarily

    $.ajax({
        url: form.attr("action"),
        type: form.attr("method"),
        data: form.serialize(),
        success: function (response) {
            if (response.info) {
                // Hide only the modal that contains this form
                form.closest(".modal").modal("hide");
                toastr.success(response.message || "Successfully done!");
                // reload full page
                window.location.href = window.location.href;
            } else {
                form.closest(".modal").modal("hide");
                toastr.error(response.message || "Something went wrong!");
            }
        },
        error: function (xhr) {
            toastr.error("Error occurred: " + xhr.responseText);
        },
        complete: function () {
            btn.prop("disabled", false); // re-enable submit button
        }
    });
});



function Delete(TableName, ColName, id) {
    if (!confirm("Are you sure you want to delete this row?"))
        return;

    $("#loadingSpinner").show();

    $.ajax({
        url: '/Crud/Delete/',
        type: "POST",
        data: { TableName: TableName, ColName: ColName, id: id },
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

