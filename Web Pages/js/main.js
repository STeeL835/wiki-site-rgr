/* globals $:false jQuery:false document:false */

(function() {
	
	$('.sidebar-control').on('click', function () {
			$('.sidebar').toggleClass('toggled');
		});
	
	$('#login-control').on('click', function () {
			$('.login-form').toggleClass('toggled');
		});
	
	$('[data-toggle="popoverx"]').popover({animation:true, content:$('[data-toggle="popoverx"]').parent().find('output').html(), html:true});
	
	$('[data-toggle=tooltip]').tooltip();
})();
