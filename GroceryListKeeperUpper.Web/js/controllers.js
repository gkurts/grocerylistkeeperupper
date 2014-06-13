appControllers.controller('indexController', ['$scope',
	function indexController($scope) {
	}
]);

appControllers.controller('navController', ['$scope', 'sessionService',
	function navController($scope, sessionService) {
	    $scope.username = sessionService.get('username');
	}
]);

appControllers.controller('loginController', ['$scope', 'userService', '$location', 'sessionService',
	function loginController($scope, userService, $location, sessionService){
	    $scope.credentials = { username: '', password: '' };
	    $scope.register = { fullname: '', email: '', password: '' };
	    $scope.message = '';

	    $scope.doRegister = function () {
	        console.log($scope.register);
            userService.register($scope.register)
                .success(function (data) {
                    console.log(data);
                    $location.path('/login');
                })
                .error(function (data, status) {
                    console.log(data);
                    console.log(status);
                    failLogin();
                });
        }

		$scope.doLogin = function () {
		    userService.login($scope.credentials)
				.success(function(data){
					console.log(data);
					if (data.status === 401)
						failLogin();
					else
						doLogin($scope.credentials, data);
				})
				.error(function(data, status){
					console.log(data);
					console.log(status);
					failLogin();
				});
		}

		function doLogin(credentials, response) {
	        sessionService.store('token', response.token);
	        sessionService.store('username', credentials.username);
	        $location.path('/');
	    }
	    
	    function failLogin() {
	        sessionService.clear();
	        $scope.message = 'Invalid Login!';
	        $scope.credentials.password = '';
	    }
	}
]);



appControllers.controller('listController', ['$scope', 'listService', '$location',
	function locationListController($scope, listService, $location) {
	    $scope.lists = [];
	    $scope.newlisttitle = '';

	    $scope.$on('listdeleted', function (event, listid) {
	        _.each($scope.lists, function(list, i) {
	            if (list.id == listid) {
	                $scope.lists.splice(i, 1);
	            }
	        });
	    });
        
        $scope.newlist = function() {
            listService.addList({ 'title': $scope.newlisttitle })
                .success(function(data) {
                    $scope.lists.push(data);
                    $scope.newlisttitle = '';
                    $location.path('/lists/' + data.id);
                })
                .error(function (data, status) {
                    console.log(data);
                    console.log(status);
                });
        }

	    listService.getAllLists()
			.success(function(data){
				$scope.lists = data;
			})
			.error(function(data, status){
				console.log(data);
				console.log(status);
			});
	}
]);

appControllers.controller('listItemController', ['$scope', 'listService', '$routeParams', '$rootScope', '$location',
	function listItemController($scope, listService, $routeParams, $rootScope, $location) {
	    var listid = $routeParams.listid;
	    $scope.list = {};
	    $scope.items = [];

	    $scope.changeQty = function(itemid, qty) {
	        _.each($scope.items, function(item, i) {
	            if (item.id == itemid) {
	                item.qty = qty;

	                listService.updateItem(listid, item)
	                    .success(function(data) {
	                        console.log(data);
	                    })
	                    .error(function(data, status) {
	                        console.log(data);
	                        console.log(status);
	                    });
	            }
	        });
	    };

	    $scope.updateItem = function(itemid, title) {
	        _.each($scope.items, function(item, i) {
	            if (item.id == itemid) {
	                item.title = title;

	                listService.updateItem(listid, item)
	                    .success(function(data) {
	                        console.log(data);
	                    })
	                    .error(function(data, status) {
	                        console.log(data);
	                        console.log(status);
	                    });
	            }
	        });
	    };

	    $scope.gotItem = function (itemid, got) {
	        _.each($scope.items, function (item, i) {
	            if (item.id == itemid) {
	                item.got = got;

	                listService.updateItem(listid, item)
	                    .success(function (data) {
	                        console.log(data);
	                    })
	                    .error(function (data, status) {
	                        console.log(data);
	                        console.log(status);
	                    });
	            }
	        });
	    };

	    $scope.deleteItem = function (itemid) {
	        listService.deleteItem(listid, itemid)
	            .success(function (data) {
	                _.each($scope.items, function(item, i) {
                        if (item.id == itemid) {
                            $scope.items.splice(i, 1);
                        }
	                });
	            })
	            .error(function (data, status) {
	                console.log(data);
	                console.log(status);
	            });
	    }

	    $scope.addItem = function () {
	        var itemtext = $scope.itemtext;
            var item = { 'title': itemtext, 'qty': 1, 'listId': $scope.list.id };

	        listService.addItem(listid, item)
                .success(function(data) {
                    $scope.items.push(data);
	                $scope.itemtext = '';
	            })
			    .error(function (data, status) {
			        console.log(data);
			        console.log(status);
			    });
            
	    };
        
	    $scope.deletelist = function () {
            listService.deleteList($scope.list.id)
                .success(function() {
                    $rootScope.$broadcast('listdeleted', listid);
                    $location.path('/');
                })
			    .error(function (data, status) {
			        console.log(data);
			        console.log(status);
			    });
        }

	    listService.getList(listid)
			.success(function (data) {
	            $scope.list = data.list;
			    $scope.items = data.items;
			})
			.error(function (data, status) {
			    console.log(data);
			    console.log(status);
			});
	}
]);