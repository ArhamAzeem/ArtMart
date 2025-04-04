$(document).ready(function (){
    if ($('.common-form').length > 0) {
        $('.common-form').submit(function(event) {
            event.preventDefault(); 

            var form = $(this);
            $('.invalid-feedback', form).slideUp(); 
            $('.form-control', form).removeClass('is-invalid');
            
            var form_data = new FormData(form[0]); 
            var action_url = form.attr('action');
            var method = form.attr('method');
            var reverse = form.attr('reverse');
            var submitButton = $(this);
            var spinner = submitButton.find('.spinner-border');
            
            submitButton.prop('disabled', true);
            spinner.show();

            $.ajax({
                type: method,
                url: action_url,
                dataType: 'json',
                cache: false,
                contentType: false,
                processData: false,
                data: form_data,
                success: function(response) {
                    console.log(response);
                    if(response && response.success){
                        if (response.success_message) {
                            swal(response.success_message.title, response.success_message.text, response.success_message.type)
                            .then(function() {
                                window.location.href = reverse; 
                            });
                        } else {
                            swal("Success!", "The operation was completed successfully.", "success");
                        }                      
                    } else {
                        if (response.error_validation) {
                            $.each(response.error_validation, function(field, message) {
                                field = field.replace(/\./g, '-');
                                var $field = $('[name="' + field + '"]', form);
                                $field.siblings('.invalid-feedback').slideDown().text(message);
                                $field.addClass('is-invalid');
                            });
                        } else if (response.error_message) {
                            swal(response.error_message.title, response.error_message.text, response.error_message.type);
                        } else if (response.error_exception) {
                            console.log(response.error_exception);
                            swal("Server Error", "An unexpected error occurred. Please try again later.", "error");
                        } else {
                            swal("Error", "An unknown error occurred. Please try again later!", "error");
                        }
                    }

                    submitButton.prop('disabled', false);
                    spinner.hide();
                },
                error: function(xhr) {
                    if (xhr.status === 0) {
                        swal("No Internet Connection", "It seems you're offline. Please check your connection and try again!", "error");
                    } else if (xhr.responseJSON && xhr.responseJSON.error) {
                        swal("Server Error", xhr.responseJSON.error, "error");
                    } else {
                        swal("Error", "An error occurred while processing your request. Please try again later!", "error");
                    }

                    submitButton.prop('disabled', false);
                    spinner.hide();
                }
            });
        });
    }

    if ($('.common-status').length > 0) {
        $(document).on('click', '.common-status', function (e) {
            e.preventDefault(); // Prevent default anchor behavior

            var status_url = $(this).attr('href');

            swal({
                title: "Are you sure?",
                text: "Once deleted, you will not be able to recover this data!",
                icon: "warning",
                buttons: true,
                dangerMode: true,
            }).then((willUpdate) => {
                if (willUpdate) {
                    $.ajax({
                        type: "GET",
                        url: status_url,
                        dataType: 'json',
                        success: function(response) {
                            if (response && response.success) {
                                swal("Poof! Your status has been updated!", {
                                    icon: "success",
                                }).then(function() {
                                    location.reload();
                                });
                            } else {
                                swal("Status Not Updated. Please Try Again Later!", {
                                    icon: "error",
                                });
                            }
                        },
                        error: function(xhr) {
                            if (xhr.status === 0) {
                                swal("Error", "Network error: Please check your internet connection and try again!", "error");
                            } else if (xhr.responseJSON && xhr.responseJSON.error) {
                                swal("Error", xhr.responseJSON.error, "error");
                            } else {
                                swal("Error", "Error: Please try again later!", "error");
                            }
                        }
                    });
                } else {
                    swal("Your Status is safe!", {
                        icon: "error",
                    });
                }
            });
        });
    }

    if ($('.common-delete').length > 0) {
        $(document).on('click', '.common-delete', function (e) {
            e.preventDefault(); // Prevent default anchor behavior

            var delete_url = $(this).attr('href');

            swal({
                title: "Are you sure?",
                text: "Once deleted, you will not be able to recover this data!",
                icon: "warning",
                buttons: true,
                dangerMode: true,
            }).then((willDalete) => {
                if (willDalete) {
                    $.ajax({
                        type: "GET",
                        url: delete_url,
                        dataType: 'json',
                        success: function(response) {
                            if (response && response.success) {
                                swal("Poof! Your data has been deleted!", {
                                    icon: "success",
                                }).then(function() {
                                    location.reload();
                                });
                            } else {
                                swal("Data Not Deleted. Please Try Again Later!", {
                                    icon: "error",
                                });
                            }
                        },
                        error: function(xhr) {
                            if (xhr.status === 0) {
                                swal("Error", "Network error: Please check your internet connection and try again!", "error");
                            } else if (xhr.responseJSON && xhr.responseJSON.error) {
                                swal("Error", xhr.responseJSON.error, "error");
                            } else {
                                swal("Error", "Error: Please try again later!", "error");
                            }
                        }
                    });
                } else {
                    swal("Your Data is safe!", {
                        icon: "error",
                    });
                }
            });
        });
    }
    if ($('.common-restore').length > 0) {
        $(document).on('click', '.common-restore', function (e) {
            e.preventDefault(); // Prevent default anchor behavior

            var delete_url = $(this).attr('href');

            swal({
                title: "Are you sure you want to restore this record?",
                // text: "Once deleted, you will not be able to recover this data!",
                icon: "warning",
                buttons: true,
                dangerMode: true,
            }).then((willRestore) => {
                if (willRestore) {
                    $.ajax({
                        type: "GET",
                        url: delete_url,
                        dataType: 'json',
                        success: function(response) {
                            if (response && response.success) {
                                swal("Your data has been restored!", {
                                    icon: "success",
                                }).then(function() {
                                    location.reload();
                                });
                            } else {
                                swal("Data Not Restored. Please Try Again Later!", {
                                    icon: "error",
                                });
                            }
                        },
                        error: function(xhr) {
                            if (xhr.status === 0) {
                                swal("Error", "Network error: Please check your internet connection and try again!", "error");
                            } else if (xhr.responseJSON && xhr.responseJSON.error) {
                                swal("Error", xhr.responseJSON.error, "error");
                            } else {
                                swal("Error", "Error: Please try again later!", "error");
                            }
                        }
                    });
                } else {
                    swal("Your Data is safe!", {
                        icon: "error",
                    });
                }
            });
        });
    }

    if ($('#myTable').length > 0 && !$.fn.DataTable.isDataTable('#myTable')) {
        $('#myTable thead tr')
            .clone(false)
            .addClass('filters')
            .appendTo('#myTable thead');

        var table = $('#myTable').DataTable({
            scrollX: true,
            // responsive: true,
            dom: 'Bfrtip',
            buttons: [
                'print',
                'excelHtml5'
            ], // No buttons
            "order": false,
            stateSave: true,
            orderCellsTop: true,
            fixedHeader: false,
            initComplete: function() {
                var api = this.api();

                // For each column
                api
                    .columns()
                    .eq(0)
                    .each(function(colIdx) {
                        // Set the header cell to contain the input element
                        var cell = $('.filters th').eq(
                            $(api.column(colIdx).header()).index()
                        );
                        var title = $(cell).text().trim();
                        if (title === 'Action') {
                            $(cell).html('<button id="clearFiltersBtn" class="btn btn-danger" style="width:100%; justify-content: center; min-width: 7rem;">Clear Filter</button>');
                            $(cell).addClass('text-center');
                        } else {
                            $(cell).html('<input type="text" class="form-control" placeholder="' + title + '" />');

                            // On every keypress in this input
                            $('input', $(cell))
                                .off('keyup change')
                                .on('change', function(e) {
                                    // Get the search value
                                    $(this).attr('title', $(this).val());
                                    var regexr = '({search})'; //$(this).parents('th').find('select').val();

                                    var cursorPosition = this.selectionStart;
                                    // Search the column for that value
                                    api
                                        .column(colIdx)
                                        .search(
                                            this.value != '' ?
                                            regexr.replace('{search}', '(((' + this.value +
                                                ')))') :
                                            '',
                                            this.value != '',
                                            this.value == ''
                                        )
                                        .draw();
                                })
                                .on('keyup', function(e) {
                                    e.stopPropagation();

                                    $(this).trigger('change');
                                    $(this)
                                        .focus()[0]
                                        .setSelectionRange(cursorPosition, cursorPosition);
                                });
                        }
                    });

                // Clear filters and state button functionality
                $('#clearFiltersBtn').on('click', function() {
                    $('.filters input').val('').trigger('change'); // Clear input values and trigger change event
                    api.search('').draw(); // Clear DataTable search and redraw
                    // Check if there is a saved state
                    if (table.state.loaded()) {
                        table.state.clear(); // Clear the saved state in local storage or session storage
                        window.location.reload(); // Reload the page to apply the clear state
                    }
                });
            },
        });
    

        if($('.table-image-popup').length){
            table.$('.table-image-popup').magnificPopup({
                type: 'image',
                zoom: {
                    enabled: true,
                    duration: 300, // Duration for the zoom animation
                    easing: 'ease-in-out', // Easing type for animation
                    opener: function(openerElement) {
                        return openerElement.is('img') ? openerElement : openerElement.find('img');
                    }
                }
            });
        }
        // $('#myTable thead tr')
        // .clone(false)
        // .addClass('filters')
        // .appendTo('#myTable thead');

        // var table = $('#myTable').DataTable({
        //     responsive: true,
        //     dom: 'Bfrtip',
        //     buttons: [
        //         'print',
        //         'excelHtml5'
        //     ],
        //     stateSave: true,
        //     orderCellsTop: true,
        //     fixedHeader: false,
        //     initComplete: function() {
        //         var api = this.api();

        //         // For each column
        //         api.columns().eq(0).each(function(colIdx) {
        //             var cell = $('.filters th').eq($(api.column(colIdx).header()).index());
        //             var title = $(cell).text().trim();

        //             if (title === 'Action') {
        //                 $(cell).html('<button id="clearFiltersBtn" class="btn btn-danger" style="width:100%; justify-content: center; min-width: 7rem;">Clear Filter</button>');
        //                 $(cell).addClass('text-center');
        //             } else {
        //                 $(cell).html('<input type="text" class="form-control" placeholder="' + title + '" />');

        //                 $('input', $(cell))
        //                     .off('keyup change')
        //                     .on('change', function(e) {
        //                         var inputVal = this.value;
        //                         api.column(colIdx).search(inputVal).draw();
        //                     })
        //                     .on('keyup', function(e) {
        //                         e.stopPropagation();
        //                         $(this).trigger('change');
        //                     });
        //             }
        //         });

        //         // Clear filters and state button functionality
        //         $('#clearFiltersBtn').on('click', function() {
        //             $('.filters input').val('').trigger('change');
        //             api.search('').draw();
        //             if (table.state.loaded()) {
        //                 table.state.clear();
        //                 window.location.reload();
        //             }
        //         });
        //     },
        //     columnDefs: [
        //         {
        //             targets: '_all',
        //             render: function(data, type, row, meta) {
        //                 if (type === 'filter' && $(data).is('input')) {
        //                     return $(data).val(); // Return the value of the input for filtering
        //                 }
        //                 return data; // Return the default data for other types
        //             }
        //         }
        //     ]
        // });

        
    }
});