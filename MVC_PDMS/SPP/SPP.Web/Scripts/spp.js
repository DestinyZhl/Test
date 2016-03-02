//当SPP存在则返回SPP，如果不存在则返回{}
var SPP = SPP || {};
SPP.Utility = SPP.Utility || {};

//// SPP Global Settings
SPP.Utility.Settings = {
    
    Datatables: {
        dom: 'rt',
        paging: false,
        searching: false,
        ordering: true,
        autoWidth: false,
        //No initial order
        order: []
    },
    Pages: {
        pageSize: 10,
        pageBtnCount: 11,
        showFirstLastBtn: true,
        firstBtnText: "First",
        lastBtnText: "Last",
        prevBtnText: "Previous",
        nextBtnText: "Next;",
        jumpBtnText: 'Go',
        infoFormat: 'Showing {start} to {end} of {total} entires',
        loadFirstPage: true,
        remote: {
            url: null,
            params: null,
            callback: null,
            success: null,
            beforeSend: null,
            complete: null,
            pageIndexName: 'PageNumber',
            pageSizeName: 'PageSize',
            totalName: 'TotalItemCount'
        },
        showInfo: true,
        showJump: true,
        showPageSizes: true,
        pageSizeItems: [10, 25, 50],
        debug: false
    },
    PopoverInTableAction: {
        template: ['<div class="action-cnt popover">',
                    '<div class="arrow"></div>',
                        '<div class="popover-content">',
                    '</div>',
                '</div>'].join('')
    },
    Datetimepicker: { format: 'yyyy-mm-dd', minView: 2, autoclose: true },
    CriteriaPreviewWidth: 120,
    ValidateMessage: {
        twoDateError: "Date to must great than Date from",
        fourDateError:""
    }
};

//// Hide all Unauthorized Elements
//// call it after page loaded and ajax successed.
SPP.Utility.UnauthorizedElements = (function () {
    return {
        Hide: function () {
            var pageId = $('#system_page_id');
            if (pageId.length > 0) {
                $.getJSON(pageId.data('url')
                    , { pageUrl: pageId.data('id') }
                    , function (data) {
                        var needHideElements = data.PageElements.split(',');
                        $.each(needHideElements, function (index, element) {
                            var target = $(element);
                            if (target.length > 0 && !element.startWith('_')) {
                                target.hide();
                            }
                        });//end each
                        data.type = 'UnauthorizedElements';
                    });
            }//end if
        }//end hide
    }
})();

//// Extend jQuery Ajax for spp custom
jQuery(function ($) {

    var _ajax = $.ajax;

    $.ajax = function (opt) {

        //back ajax
        var fn = {
            error: function (XMLHttpRequest, textStatus, errorThrown) { },
            success: function (data, textStatus) { }
        }
        if (opt.error) {
            fn.error = opt && opt.error || function (a, b) { };
        }
        if (opt.success) {
            fn.success = opt && opt.success || function (a, b) { };
        }

        var _opt = $.extend(opt, {
            error: function (xhr, textStatus, errorThrown) {
                //Error参数
                //jqXHR对象、描述发生错误类型的一个字符串 和 捕获的异常对象。
                //如果发生了错误，错误信息（第二个参数）除了得到null之外，还可能是"timeout", "error", "abort" ，和 "parsererror"。 
                //当一个HTTP错误发生时，errorThrown 接收HTTP状态的文本部分，比如： "Not Found" 或者 "Internal Server Error."。
                //从jQuery 1.5开始, 在error设置可以接受函数组成的数组。每个函数将被依次调用。 
                //注意：此处理程序在跨域脚本和JSONP形式的请求时不被调用 
                fn.error(XMLHttpRequest, textStatus, errorThrown);
            },
            success: function (data, textStatus) {

                if (data.error) {

                    SPP.Utility.MessageBox.error(data.message);
                    return;
                } else {

                    fn.success(data, textStatus);
                    if (data.type != 'UnauthorizedElements') {
                        SPP.Utility.UnauthorizedElements.Hide();
                    }
                }
            }
        });

        _ajax(_opt);
    };
});

SPP.Utility.Tools = (function () {
    var tools = {};
    function _transferDate(assigntime) {

        if (!assigntime)
            return null;
        else {
            var reg = new RegExp('-', 'g');
            assigntime = assigntime.replace(reg, '/');//正则替换
            assigntime = new Date(parseInt(Date.parse(assigntime), 10));
            return assigntime;
        }
    }
    /// <summary>
    /// 判断html element是否含有特定class 
    /// </summary>
    tools.HasClass = function (el, elClassName) {
        return el.classList ? el.classList.contains(elClassName) : el.className.indexOf(elClassName) > -1;
    };
    /// <summary>
    /// 日期转换
    /// </summary>
    tools.TransferDate = function (assigntime) {

        return this._transferDate(assigntime);
    };
    /// <summary>
    /// 比较传入时间的大小
    /// </summary>
    /// <param name="StartDate">开始时间</param>
    /// <param name="EndDate">结束时间</param>
    /// <returns>true/false</returns>
    tools.DateCompare = function (StartDate, EndDate) {

        StartDate = _transferDate(StartDate);
        EndDate = _transferDate(EndDate);
        if (StartDate == null || EndDate == null) {
            return true
        } else {
            return StartDate <= EndDate
        }
    };
    /// <summary>
    /// 后台验证两段时间是否拥有包含关系
    /// </summary>
    /// <param name="SubStart">待验证开始时间</param>
    /// <param name="SubEnd">待验证结束时间</param>
    /// <param name="HeadStart">验证标准开始时间</param>
    /// <param name="HeadEnd">验证标准结束时间</param>
    /// <returns>Pass：验证通过，NoNeedVerify：没有验证的必要，Other：验证不通过</returns>
    tools.DateCompareInterval = function (SubStart, SubEnd, HeadStart, HeadEnd) {
        SubStart = _transferDate(SubStart);
        SubEnd = _transferDate(SubEnd);
        HeadStart = _transferDate(HeadStart);
        HeadEnd = _transferDate(HeadEnd);
        if (SubStart == null && SubEnd == null)
            return "NoNeedVerify_SubStartAndEndDateNull";
        else if (SubStart != null && SubEnd != null && SubStart > SubEnd)
            return "NoNeedVerify_SubEndLowerThanSubStartDate";
        else if (HeadStart != null && HeadEnd != null && HeadStart > HeadEnd)
            return "NoNeedVerify_HeadEndLowerThanHeadStartDate";
        else {
            if (SubStart == null && SubEnd != null && HeadStart == null && HeadEnd != null && SubEnd > HeadEnd)
                return "SubEndLowerThanHeadEndDate";
            else if (SubStart == null && SubEnd != null && HeadStart != null && HeadEnd == null)
                return "SubStartIsNullHeadStartNotNull";
            else if (SubStart == null && SubEnd != null && HeadStart != null && HeadEnd != null)
                return "SubStartIsNullHeadStartNotNull";
            else if (SubStart != null && SubEnd == null && HeadStart == null && HeadEnd != null)
                return "SubEndIsNullHeadEndNotNull";
            else if (SubStart != null && SubEnd == null && HeadStart != null && HeadEnd != null)
                return "SubEndIsNullHeadEndNotNull";
            else if (SubStart != null && SubEnd == null && HeadStart != null && HeadEnd == null && SubStart < HeadStart)
                return "SubStartLowerThanHeadStartDate";
            else if (SubStart != null && SubEnd != null && HeadStart == null && HeadEnd != null && SubEnd > HeadEnd)
                return "SubEndLowerThanHeadEndDate";
            else if (SubStart != null && SubEnd != null && HeadStart != null && HeadEnd == null && SubStart < HeadStart)
                return "SubStartLowerThanHeadStartDate";
            else if (SubStart != null && SubEnd != null && HeadStart != null && HeadEnd != null && SubStart < HeadStart)
                return "SubStartLowerThanHeadStartDate";
            else if (SubStart != null && SubEnd != null && HeadStart != null && HeadEnd != null && SubEnd > HeadEnd)
                return "SubEndLowerThanHeadEndDate";
            else
                return "Pass";
        }
    };
    return tools;
})();

SPP.Utility.Datatables = (function () {

    var customOptions = {},
        seqIndex = -1,
        nosortIndexs = [],
        $targetTable = null,
        $targetTableThead = null;

    var _init = function () {

        if (window.matchMedia("(max-width: 1200px)").matches) {

            if ($targetTable.find('tbody>tr').eq(0).find('.td-col-detail').length == 0) {
                $targetTable.find('thead tr').prepend('<th>&nbsp;</th>');
                $targetTable.find('tfoot tr').prepend('<th>&nbsp;</th>');
                //添加colums
                customOptions.columns.unshift({
                    data: null,
                    orderable: false,
                    defaultContent: ' ',
                    className: "text-center text-success td-col-detail",
                    createdCell: function (td, cellData, rowData, row, col) {
                        $(td).html('<i class="fa fa-plus-circle td-detail-icon"></i>');
                    }
                });
            }

        };

        seqIndex = _getSeqIndex();
        nosortIndexs = _getNosortIndexs();
        $targetTableThead = _getTargetTableThead();
    };

    var _getTargetTableThead = function () {

        return $targetTableThead = $targetTableThead || $targetTable.find('thead tr')
    };

    var _getSeqIndex = function () {

        if (seqIndex === -1) {
            var targetCol = _getTargetTableThead().find('.table-col-seq');
            if (targetCol.length > 0) {
                seqIndex = targetCol.index();
            };
        }

        return seqIndex;
    };

    var _getNosortIndexs = function () {

        var $targetTableTheadCols = $targetTableThead.find('th');

        $.each($targetTableTheadCols, function (idx, val) {

            var flag = SPP.Utility.Tools.HasClass(val, 'nosort');

            if (flag) {
                nosortIndexs.push(idx);
            };
        });

        return nosortIndexs;
    };

    var _setOptions = function () {

        if (customOptions.columns) {

            if (nosortIndexs.length > 0) {
                //set nosort column
                $.each(nosortIndexs, function (index, el) {
                    var targetCol = customOptions.columns[el];
                    targetCol["orderable"] = false;
                    targetCol["data"] = null;
                });
            };


        };

        return customOptions;
    };

    var _resize = function (t) {
        //hide cols
        var cols = customOptions.columns,
            indexArrary = [],
            visibleColClassNames = [],
            invisibleColNames = [],
            that = this;

        var setColVisiblity = function (visibleColClassNames, invisibleColNames) {

            $.each(cols, function (index, el) {

                for (var i = 0; i < visibleColClassNames.length; i++) {

                    var className = visibleColClassNames[i];
                    if (el.className && el.className.indexOf(className) > -1) {

                        t.columns('.' + className).visible(true);
                    };
                };

                for (var i = 0; i < invisibleColNames.length; i++) {

                    var className = invisibleColNames[i];
                    if (el.className && el.className.indexOf(className) > -1) {

                        t.columns('.' + className).visible(false);
                    };
                };
            });

            return indexArrary;
        };

        if (window.matchMedia("(max-width:768px)").matches) {

            visibleColClassNames = ['min-col-xs', 'td-col-detail'];
            invisibleColNames = ['min-col-lg', 'min-col-md', 'min-col-sm'];

        } else if (window.matchMedia("(max-width:992px)").matches) {

            visibleColClassNames = ['min-col-sm', 'min-col-xs', 'td-col-detail'];
            invisibleColNames = ['min-col-lg', 'min-col-md'];

        } else if (window.matchMedia("(max-width: 1200px)").matches) {

            visibleColClassNames = ['min-col-md', 'min-col-sm', 'min-col-xs', 'td-col-detail'];
            invisibleColNames = ['min-col-lg'];

        } else {

            visibleColClassNames = ['min-col-md', 'min-col-sm', 'min-col-xs', 'min-col-lg'];
            //invisibleColNames = ['td-col-detail'];
        }

        return setColVisiblity(visibleColClassNames, invisibleColNames);
    };

    return {

        SetDatetable: function (datatableConfig) {

            var that = this,
                tableId = datatableConfig.tableId,
                options = datatableConfig.tableOptions;

            //remove # if tableid start with #
            if (tableId.indexOf('#') == 0) {
                tableId = tableId.substring(1);
            }

            $targetTable = $('#' + tableId);
            customOptions = options;

            _init();

            var finalOptions = _setOptions();
            var finalTable = $targetTable.DataTable(finalOptions);

            //set seq
            if (seqIndex > -1) {

                finalTable
					.on('order.dt', function () {

					    finalTable.column(seqIndex, { order: 'applied' })
			        	.nodes()
			        	.each(function (cell, i) {
			        	    cell.innerHTML = i + 1;
			        	});
					})
			        .draw();
            };

            _resize(finalTable);

            $(document).on('click', '.td-detail-icon', function () {
                //get row
                var rowIdx = $(this).closest('tr').index();
                var rowCells = finalTable.cells(rowIdx, '')[0];
                var rowDataHtml = '';
                var modalId = tableId + '_modal';

                //build content of modal body
                for (var i = seqIndex + 1; i < rowCells.length; i++) {

                    var title = finalTable.columns(i).header();
                    var cellData = finalTable.cells(rowIdx, i).data();

                    rowDataHtml += '<div class="form-group">' +
                                    '<label class="col-sm-4 control-label">' + $(title).html() + '</label>' +
                                    '<div class="col-sm-8">' +
                                      '<p class="form-control-static">' + cellData[0] + '</p>' +
                                    '</div>' +
                                  '</div>';
                };

                if ($('#' + modalId).length === 0) {

                    var modalHtml = '<div class="modal fade" id="' + modalId + '" tabindex="-1" role="dialog">' +
                                      '<div class="modal-dialog" role="document">' +
                                        '<div class="modal-content">' +
                                          '<div class="modal-header">' +
                                            '<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>' +
                                            '<h4 class="modal-title" id="myModalLabel">Row Details</h4>' +
                                          '</div>' +
                                          '<div class="modal-body">' +
                                            '<div class="form-horizontal">' + rowDataHtml + '</div>' +
                                          '</div>' +
                                          '<div class="modal-footer">' +
                                            '<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>' +
                                          '</div>' +
                                        '</div>' +
                                      '</div>' +
                                    '</div>';

                    $('body').append(modalHtml);

                } else {

                    $('#' + modalId + " .form-horizontal").empty().append(rowDataHtml);
                }

                $('#' + modalId).modal('show');
            });

            //$(window).on('resize.dtr', finalTable.settings()[0].oApi._fnThrottle(function () {

            //    SPP.Utility.Datatables.ResizeDatatable(finalTable);
            //}));

            ////Destroy event handler
            //finalTable.on('destroy.dtr', function () {
            //    $(window).off('resize.dtr');
            //});

            return finalTable;
        },
        ResizeDatatable: function (t) {
            _resize(t);
        }
    }
})();

SPP.Utility.Pages = (function () {
    return {
        Set: function (config) {
            var datatable;
            var pageId = config.pageId;
            if (pageId.indexOf('#') < 0) {
                pageId = '#' + pageId;
            }

            $(config.pageId).page({
                remote: {
                    url: config.remoteUrl,
                    params: config.searchParams,
                    success: function (data, pageIndex) {

                        config.tableOptions.aaData = data.Items;
                        config.tableOptions.destroy = true;

                        datatable = SPP.Utility.Datatables.SetDatetable(config);
                    }
                }//end remote
            });//end page

        }//end SetPages
    }
})();

SPP.Utility.MessageBox = {

    info: function (message, callback) {

        if ($('#infoModal').length == 0) {
            var modalHtml = '<div class="modal fade" id="infoModal" tabindex="-1" role="dialog">' +
                                '<div class="modal-dialog" role="document">' +
                                    '<div class="modal-content">' +
                                        '<div class="modal-header">' +
                                            '<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>' +
                                            '<h4 class="modal-title" id="myModalLabel">Info</h4>' +
                                        '</div>' +
                                        '<div class="modal-body"></div>' +
                                        '<div class="modal-footer">' +
                                            '<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>' +
                                        '</div>' +
                                    '</div>' +
                                '</div>' +
                            '</div>';
            $('body').append(modalHtml);
        }
        var modal = $("#infoModal").modal();
        $("#infoModal .modal-body").text(message);
        $("#infoModal .modal-footer .btn").unbind("click");
        $("#infoModal .modal-footer .btn").bind("click", function () {
            if (callback) {
                callback();
                modal.hide();
            }
        });
        modal.show();
    },

    error: function (message, callback) {
        if ($('#errorModal').length == 0) {
            var modalHtml = '<div class="modal fade modal-danger" id="errorModal" tabindex="-1" role="dialog">' +
                               '<div class="modal-dialog" role="document">' +
                                   '<div class="modal-content">' +
                                       '<div class="modal-header">' +
                                           '<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>' +
                                           '<h4 class="modal-title" id="myModalLabel">Attention</h4>' +
                                       '</div>' +
                                       '<div class="modal-body"></div>' +
                                       '<div class="modal-footer">' +
                                           '<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>' +
                                       '</div>' +
                                   '</div>' +
                               '</div>' +
                           '</div>';
            $('body').append(modalHtml);
        }
        var modal = $("#errorModal").modal();
        $("#errorModal .modal-body").text(message);
        $("#errorModal .modal-footer .btn").unbind("click");
        $("#errorModal .modal-footer .btn").bind("click", function () {
            if (callback) {
                callback();
                modal.hide();
            }
        });
        modal.show();
    },

    confirm: function (message, callbackYes, callbackNo) {
        if ($('#confirmModal').length == 0) {
            var modalHtml = '<div class="modal fade" id="confirmModal" tabindex="-1" role="dialog">' +
                               '<div class="modal-dialog" role="document">' +
                                   '<div class="modal-content">' +
                                       '<div class="modal-header">' +
                                           '<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>' +
                                           '<h4 class="modal-title" id="myModalLabel">Confirm</h4>' +
                                       '</div>' +
                                       '<div class="modal-body"></div>' +
                                       '<div class="modal-footer">' +
                                           '<button type="button" class="btn btn-primary">Yes</button>' +
                                           '<button type="button" class="btn btn-default" data-dismiss="modal">No</button>' +
                                       '</div>' +
                                   '</div>' +
                               '</div>' +
                           '</div>';
            $('body').append(modalHtml);
        }
        var modal = $("#confirmModal").modal();
        $("#confirmModal .modal-body").text(message);
        $("#confirmModal .modal-footer .btn-default").unbind("click");
        $("#confirmModal .modal-footer .btn-default").bind("click", function () {
            if (callbackNo) {
                callbackNo();
            }
            $("#confirmModal").modal('hide');
        });
        $("#confirmModal .modal-footer .btn-primary").unbind("click");
        $("#confirmModal .modal-footer .btn-primary").bind("click", function () {
            if (callbackYes) {
                callbackYes();
            }
            $("#confirmModal").modal('hide');
        });
        modal.show();
    }
}

SPP.Utility.Criteria = (function () {

    var _clearCriteria = function () {

        var $moreCriteriaCnt = $('#js_criteria_more_cnt'),
            $previewCriteriaCnt = $('#js_search_keywords');

        if ($moreCriteriaCnt.length > 0) {
            $moreCriteriaCnt.remove();
        }
        $previewCriteriaCnt.html("");
    }

    return {

        Init: function () {

            $(document).on('click', '.btn-search-keywords-more', function () {

                var $previewCriteriaCnt = $('#js_search_keywords').hide();

                var template = '<div class="alert alert-dismissible fade in" id="js_criteria_more_cnt" role="alert" >' +
                                    '<button type="button" class="close js-criteria-more-close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">×</span></button>' +
                                    '<strong>Criteria:</strong> ' + $previewCriteriaCnt.data('criteria') +
                               '</div>';
                $('#js_criteria_alert_cnt').append(template);
            });

            $(document).on('click', '.js-criteria-more-close', function () {
                $('#js_search_keywords').show();
            });
        },

        Build: function () {

            _clearCriteria();

            var $searchform = $('#js_form_query'),
                $previewCriteriaCnt = $('#js_search_keywords'),
                htmlAppend = '<small><strong>Criteria: </strong>',
                criteria = [],
                criteriaStr = '';

            var $labels = $searchform.find('label').not('.radio-inline').not('.checkbox-inline').not('.error');

            $.each($labels, function (index, item) {

                var $label = $(item),
                    labelText = $label.text(),
                    queryText = '',
                    type = 'input',
                    labelData = $label.data(),
                    $labelForCnt = $label.next(),
                    targetId = $label.attr('for');

                if (labelData.type && labelData.type.toLowerCase() !== 'input') {

                    type = labelData.type.toLowerCase();

                    switch (type) {
                        case 'select':
                            queryText = $.trim($('#' + targetId + ' option:selected').text());
                            break;
                        case 'checkbox':
                            queryText = $.map(
                                            $labelForCnt.find('input[name=' + labelData.forName + ']:checked').parent()
                                            , function (val, i) {
                                                return $.trim(val.innerText);
                                            })
                                        .join(',');
                            if ($.trim(queryText).length > 0) { queryText = '[ ' + queryText + ' ]'; }
                            break;
                        case 'radio':
                            queryText = $.trim($labelForCnt.find('input[name=' + labelData.forName + ']:checked').parent().text());
                            break
                        default:
                            SPP.Utility.MessageBox.info(type + ' type is not be supported');
                            break;
                    };
                } else {
                    //default input
                    queryText = $('#' + targetId).val();
                }

                if ($.trim(queryText) !== '') {
                    criteria.push(labelText + '=' + queryText);
                }
            });

            criteriaStr = criteria.join(' & ');

            if (criteriaStr.length > 0) {

                if (criteriaStr.getStrWidth() > SPP.Utility.Settings.CriteriaPreviewWidth) {

                    htmlAppend += criteriaStr.substring(0, SPP.Utility.Settings.CriteriaPreviewWidth)
                    htmlAppend += '</small> ';
                    htmlAppend += '<a class="btn btn-default btn-xs btn-search-keywords-more">' +
                                      ' More <i class="search-more fa fa-caret-down"></i>' +
                                  '</a>';
                    $previewCriteriaCnt.data('criteria', criteriaStr).html(htmlAppend);
                } else {

                    htmlAppend += criteriaStr
                    htmlAppend += '</small>';
                    $previewCriteriaCnt.html(htmlAppend);
                };

                $previewCriteriaCnt.show();
            } else {

                $previewCriteriaCnt.html('');
            }
        },//end build

        Clear: function (appendFunction) {

            _clearCriteria();

            var $searchform = $('#js_form_query');

            $searchform.find('input:text').val('');
            $searchform.find('input[type=checkbox]:checked').prop('checked', false);
            $searchform.find('input[type=radio]:checked').prop('checked', false);
            $searchform.find('select option:eq(0)').prop('selected', true);

            if (appendFunction && jQuery.isFunction(appendFunction)) {
                appendFunction();
            }
        }//end clear
    }
})();

//#region javastript string extend
String.prototype.endWith = function (s) {
    if (s == null || s == "" || this.length == 0 || s.length > this.length)
        return false;
    if (this.substring(this.length - s.length) == s)
        return true;
    else
        return false;
    return true;
}

String.prototype.startWith = function (s) {
    if (s == null || s == "" || this.length == 0 || s.length > this.length)
        return false;
    if (this.substr(0, s.length) == s)
        return true;
    else
        return false;
    return true;
}

String.prototype.getStrWidth = function () {
    var realLength = 0;
    var len = this.length;
    var charCode = -1;
    for (var i = 0; i < len; i++) {
        charCode = this.charCodeAt(i);
        if (charCode >= 0 && charCode <= 128) {
            realLength += 1;
        } else {
            // 如果是中文则长度加1.5
            realLength += 1.5;
        }
    }
    return realLength;
}
//#endregion

//#region validate Custom Methods
jQuery.validator.addMethod(

    "StartDateGTEnd"
    , function (value, element, params) {

        //var tools = new SPP.Utility.Tools();
        return SPP.Utility.Tools.DateCompare($(params).val(), value);
    }
    , 'Date to must great than Date from'
);
//#endregion

$(document).ready(function () {

    (function () {

        $.extend($.fn.dataTable.defaults, SPP.Utility.Settings.Datatables);
        $.fn.page.defaults = SPP.Utility.Settings.Pages;
        $.fn.serializeObject = function () {
            var o = {};
            var a = this.serializeArray();
            $.each(a, function () {
                if (o[this.name]) {
                    if (!o[this.name].push) {
                        o[this.name] = [o[this.name]];
                    }
                    o[this.name].push(this.value || '');
                } else {
                    o[this.name] = this.value || '';
                }
            });
            return o;
        };

        var $searchForm = $('#js_form_query');
        if ($searchForm.data('need-validate')) {

            if ($searchForm.find('ul.form-validate-error').length == 0) {

                $searchForm.append('<div class="col-xs-12"><ul class="list-group form-validate-error"></ul></div>');
            }
            var $errorCnt = $searchForm.find($('ul.form-validate-error'));

            $searchForm.validate({
                errorContainer: $errorCnt,
                errorLabelContainer: $errorCnt,
                wrapper: 'li',
                rules: {
                    Modified_Date_End: { StartDateGTEnd: "#js_s_input_modified_from" }
                }
            });
        }

        //checkbox all
        $(document).on('click', '.js-checkbox-all', function () {

            var $self = $(this);
            $self.closest('.dataTables_wrapper').find('.js-checkbox-item').prop('checked', $self.prop('checked'));
        });

        //action popover in datatables
        $(document).popover({
            selector: '[rel=action-popover]',
            container: 'body',
            trigger: 'focus',
            content: function () {
                return $(this).parent().find('.popover-content').html();
            },
            template: SPP.Utility.Settings.PopoverInTableAction.template,
            placement: "bottom",
            html: true
        });

        $('.date').datetimepicker(SPP.Utility.Settings.Datetimepicker);
    })();
});


