/* globals $:false jQuery:false document:false */

(function() {
	
	$('.sidebar-control').on('click', function () {
			$('.sidebar').toggleClass('toggled');
		});
	
	$('#login-control').on('click', function () {
			$('.login-form').toggleClass('toggled');
		});
	
})();