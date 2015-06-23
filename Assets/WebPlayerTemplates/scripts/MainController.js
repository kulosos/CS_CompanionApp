/*
 * Main Controller Script
 *
 * @brief: Contains all initializations and execution order for scripts
 * 
 * @autor: Oliver Kulas
 * @version: 1.0
 * @date: Jun 2015
 *
 */
//-----------------------------------------------------------------------------

var D = true; // TOGGLE DEBUG CONSOLE OUTPUTS

$(document).ready(function() {

	// Execution Order
	initNavigation();
	initLayoutController();
	initDataBinding();
	initConnectionMananger();
	

});
