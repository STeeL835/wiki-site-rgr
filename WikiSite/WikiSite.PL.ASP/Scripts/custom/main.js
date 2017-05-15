/* globals $:false jQuery:false document:false */

(function() {
	
	$('.sidebar-control').on('click', function () {
			$('.sidebar').toggleClass('toggled');
		});
	
	$('#login-control').on('click', function () {
			$('.login-form').toggleClass('toggled');
	});

	// for delete popovers with button
	var list = $('[data-toggle="confirm-popover"]');
	for (var i = 0; i < list.length; i++) {
		$(list[i]).popover({ animation: true, content: $(list[i]).parent().find("output").html(), html: true });
	}

	/* Enabling tooltips */
	$('[data-toggle="tooltip"]').tooltip();
})();
