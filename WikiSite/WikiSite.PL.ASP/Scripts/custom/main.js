/* globals $:false jQuery:false document:false */

(function() {
	
	$('.sidebar-control').on('click', function () {
			$('.sidebar').toggleClass('toggled');
		});
	
	$('#login-control').on('click', function () {
			$('.login-form').toggleClass('toggled');
	});

	// for delete popovers with button
	var list = $('[data-toggle="confirm-popover"]'),
		lastPopovered,
		alertDismissed = false;
	for (var i = 0; i < list.length; i++) {
		$(list[i]).popover({ animation: true, content: $(list[i]).parent().find("output").html(), html: true });
	}

	/* Enabling tooltips */
	$('[data-toggle="tooltip"]').tooltip();

	/* result auto dismiss */
	function dismissAlert() {
		$(".result-alert").fadeTo(5000, 500).slideUp(500,
			function() {
				$(".result-alert").slideUp(500);
				alertDismissed = true;
			});
	}
	$(document).ready(function(){if (!alertDismissed){dismissAlert()}});
})();

/* If screen is 993px wide, it's probably a PC */
var IsMobile = window.matchMedia("only screen and (max-width: 992px)").matches;
