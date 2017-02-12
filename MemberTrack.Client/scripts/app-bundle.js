define('core/dtos',["require", "exports"], function (require, exports) {
    "use strict";
    var getIndex = function (array, value) {
        var index = -1, count = array.length;
        for (var i = 0; i < count; i++) {
            var item = array[i];
            if (item.id !== value) {
                continue;
            }
            index = i;
            break;
        }
        return index;
    };
    var DtoHelper = (function () {
        function DtoHelper() {
        }
        DtoHelper.prototype.insertOrUpdate = function (array, dto) {
            if (!array || !dto) {
                return;
            }
            var data = [];
            Object.assign(data, array);
            var index = getIndex(array, dto.id);
            if (index !== -1) {
                data[index] = dto;
            }
            else {
                data.push(dto);
            }
            return data;
        };
        DtoHelper.prototype.remove = function (array, id) {
            if (!array || !id) {
                return;
            }
            var index = getIndex(array, id);
            if (index < 0) {
                return;
            }
            array.splice(index, 1);
        };
        DtoHelper.prototype.getIndexOf = function (array, value) {
            return getIndex(array, value);
        };
        return DtoHelper;
    }());
    exports.DtoHelper = DtoHelper;
});

define('core/custom-events',["require", "exports"], function (require, exports) {
    "use strict";
    var DeleteItemEvent = (function () {
        function DeleteItemEvent(id) {
            this.data = id;
        }
        return DeleteItemEvent;
    }());
    exports.DeleteItemEvent = DeleteItemEvent;
    var UserEvent = (function () {
        function UserEvent(message) {
            this.data = message;
        }
        return UserEvent;
    }());
    exports.UserEvent = UserEvent;
    var SnackbarEvent = (function () {
        function SnackbarEvent(message) {
            this.data = message;
        }
        return SnackbarEvent;
    }());
    exports.SnackbarEvent = SnackbarEvent;
    var IsLoadingEvent = (function () {
        function IsLoadingEvent(message) {
            this.data = message;
        }
        return IsLoadingEvent;
    }());
    exports.IsLoadingEvent = IsLoadingEvent;
    var PersonEvent = (function () {
        function PersonEvent(message) {
            this.data = message;
        }
        return PersonEvent;
    }());
    exports.PersonEvent = PersonEvent;
});

define('services/base.service',["require", "exports", "../core/custom-events"], function (require, exports, custom_events_1) {
    "use strict";
    var API_URL = 'http://localhost:5000/membertrack/api/';
    var TOKEN_CACHE_KEY = 'membertrack.client.token.cache';
    var BaseService = (function () {
        function BaseService(client, logger, eventAggregator) {
            BaseService.logger = logger;
            BaseService.eventAggregator = eventAggregator;
            var _that = this;
            client.configure(function (config) {
                config.withBaseUrl(API_URL);
                config.withHeader('Content-Type', 'application/json');
                config.withInterceptor({
                    request: function (message) {
                        BaseService.eventAggregator.publish(new custom_events_1.IsLoadingEvent(true));
                        _that.loadTokenCache();
                        message.headers.add('Authorization', BaseService.token.token_type + " " + BaseService.token.access_token);
                        return message;
                    },
                    response: function (message) {
                        BaseService.eventAggregator.publish(new custom_events_1.IsLoadingEvent(false));
                        return message;
                    },
                    requestError: function (error) {
                        BaseService.eventAggregator.publish(new custom_events_1.IsLoadingEvent(false));
                        throw error;
                    },
                    responseError: function (error) {
                        BaseService.eventAggregator.publish(new custom_events_1.IsLoadingEvent(false));
                        throw error;
                    }
                });
            });
            this.client = client;
        }
        BaseService.prototype.setToken = function (value) {
            BaseService.token = value;
            if (!this.isLocalStorageSupported()) {
                return;
            }
            localStorage.setItem(TOKEN_CACHE_KEY, JSON.stringify(value));
        };
        BaseService.prototype.clearToken = function () {
            this.setToken({});
        };
        BaseService.prototype.isLocalStorageSupported = function () {
            return (typeof (Storage) !== 'undefined');
        };
        BaseService.prototype.isTokenAvailable = function () {
            this.loadTokenCache();
            if (BaseService.token.access_token) {
                return true;
            }
            return false;
        };
        BaseService.prototype.handleError = function (error) {
            var result = Promise.resolve(false);
            if (error.statusCode === 0) {
                var message = 'Server error (Web API). Your token might have expired. Please refresh the browser. ' +
                    'If this error continues, please contact System Administrator.';
                BaseService.logger.error(message);
                BaseService.eventAggregator.publish(new custom_events_1.SnackbarEvent(message));
                return result;
            }
            var data = JSON.parse(error.response);
            BaseService.logger.error(data.message);
            BaseService.eventAggregator.publish(new custom_events_1.SnackbarEvent(data.message));
            return result;
        };
        BaseService.prototype.loadTokenCache = function () {
            if (BaseService.token.access_token) {
                return;
            }
            if (this.isLocalStorageSupported()) {
                var tokenCache = localStorage.getItem(TOKEN_CACHE_KEY);
                if (tokenCache) {
                    BaseService.token = JSON.parse(tokenCache);
                }
                return;
            }
            BaseService.logger.warn('Local Storage is not available');
        };
        return BaseService;
    }());
    BaseService.token = {};
    exports.BaseService = BaseService;
});

var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('services/auth.service',["require", "exports", "aurelia-framework", "aurelia-http-client", "aurelia-event-aggregator", "./base.service"], function (require, exports, aurelia_framework_1, aurelia_http_client_1, aurelia_event_aggregator_1, base_service_1) {
    "use strict";
    var AuthService = (function (_super) {
        __extends(AuthService, _super);
        function AuthService(client, eventAggregator) {
            var _this = _super.call(this, client, aurelia_framework_1.LogManager.getLogger('AuthService'), eventAggregator) || this;
            _this.redirectUrl = '';
            return _this;
        }
        AuthService.prototype.getContextUser = function (refresh) {
            if (!this.contextUserPromise || refresh) {
                this.contextUserPromise = this.client.get('user/contextUser')
                    .then(function (result) { return JSON.parse(result.response); })
                    .catch(this.handleError);
            }
            return this.contextUserPromise;
        };
        AuthService.prototype.isAuthenticated = function () {
            var _this = this;
            return this.client.get('user/isAuthenticated')
                .then(function (result) {
                return _this.getContextUser().then(function () {
                    return true;
                });
            })
                .catch(function (error) {
                _this.clearToken();
                return false;
            });
        };
        AuthService.prototype.isSignedIn = function () {
            return this.isTokenAvailable();
        };
        AuthService.prototype.signIn = function (dto) {
            var _this = this;
            return this.client.post('token', dto)
                .then(function (result) { return JSON.parse(result.response); })
                .then(function (token) {
                _this.setToken(token);
                return true;
            }).catch(this.handleError);
        };
        AuthService.prototype.signOut = function () {
            this.clearToken();
            location.reload();
        };
        return AuthService;
    }(base_service_1.BaseService));
    AuthService = __decorate([
        aurelia_framework_1.autoinject,
        __metadata("design:paramtypes", [aurelia_http_client_1.HttpClient, aurelia_event_aggregator_1.EventAggregator])
    ], AuthService);
    exports.AuthService = AuthService;
});

var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('app',["require", "exports", "aurelia-framework", "aurelia-event-aggregator", "aurelia-router", "./services/auth.service", "./core/custom-events"], function (require, exports, aurelia_framework_1, aurelia_event_aggregator_1, aurelia_router_1, auth_service_1, custom_events_1) {
    "use strict";
    var App = (function () {
        function App(eventAggregator) {
            this.eventAggregator = eventAggregator;
            this.isLoading = false;
        }
        App.prototype.attached = function (argument) {
            var _this = this;
            this.eventAggregator.subscribe(custom_events_1.IsLoadingEvent, function (e) {
                if (_this.isLoading === e.data) {
                    return;
                }
                _this.isLoading = e.data;
            });
        };
        App.prototype.configureRouter = function (config, router) {
            config.title = 'MemberTrack';
            config.addPipelineStep('authorize', AuthorizeStep);
            config.mapUnknownRoutes('view-models/404.view-model');
            config.fallbackRoute('main/home');
            config.map([
                {
                    route: ['', 'main'],
                    moduleId: 'view-models/main-layout.view-model',
                    name: 'main',
                    title: 'Main',
                    caseSensitive: true
                },
                {
                    route: 'sign-in',
                    moduleId: 'view-models/sign-in.view-model',
                    name: 'sign-in',
                    title: 'Sign In',
                    caseSensitive: true
                },
                {
                    route: '401',
                    moduleId: 'view-models/401.view-model',
                    name: '401',
                    title: '401',
                    caseSensitive: true
                }
            ]);
            this.router = router;
        };
        return App;
    }());
    App = __decorate([
        aurelia_framework_1.autoinject,
        __metadata("design:paramtypes", [aurelia_event_aggregator_1.EventAggregator])
    ], App);
    exports.App = App;
    var AuthorizeStep = (function () {
        function AuthorizeStep(authService) {
            this.authService = authService;
        }
        AuthorizeStep.prototype.run = function (navigationInstruction, next) {
            var _this = this;
            var isSignInRoute = this.isSignInRoute(navigationInstruction.config.name);
            if (isSignInRoute && !this.authService.isSignedIn()) {
                return next();
            }
            if (isSignInRoute && this.authService.isSignedIn()) {
                return next.cancel();
            }
            return this.authService.isAuthenticated().then(function (isAuth) {
                if (isAuth) {
                    return _this.authService.getContextUser().then(function (user) {
                        var isAdminView = navigationInstruction.getAllInstructions().some(function (i) { return i.config.adminView; });
                        if (isAdminView && !user.isAdmin) {
                            return next.cancel(new aurelia_router_1.Redirect('401'));
                        }
                        return next();
                    });
                }
                _this.authService.redirectUrl = navigationInstruction.fragment;
                return next.cancel(new aurelia_router_1.Redirect('sign-in'));
            });
        };
        AuthorizeStep.prototype.isSignInRoute = function (routeName) {
            return routeName === 'sign-in';
        };
        return AuthorizeStep;
    }());
    AuthorizeStep = __decorate([
        aurelia_framework_1.autoinject,
        __metadata("design:paramtypes", [auth_service_1.AuthService])
    ], AuthorizeStep);
});

define('environment',["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.default = {
        debug: true,
        testing: true
    };
});

define('core/custom-log-appender',["require", "exports"], function (require, exports) {
    "use strict";
    var CustomLogAppender = (function () {
        function CustomLogAppender() {
        }
        CustomLogAppender.prototype.debug = function (logger, message) {
            var rest = [];
            for (var _i = 2; _i < arguments.length; _i++) {
                rest[_i - 2] = arguments[_i];
            }
            console.debug.apply(console, ["DEBUG [" + logger.id + "] " + message].concat(rest));
        };
        CustomLogAppender.prototype.info = function (logger, message) {
            var rest = [];
            for (var _i = 2; _i < arguments.length; _i++) {
                rest[_i - 2] = arguments[_i];
            }
            console.info.apply(console, ["INFO [" + logger.id + "] " + message].concat(rest));
        };
        CustomLogAppender.prototype.warn = function (logger, message) {
            var rest = [];
            for (var _i = 2; _i < arguments.length; _i++) {
                rest[_i - 2] = arguments[_i];
            }
            console.warn.apply(console, ["WARN [" + logger.id + "] " + message].concat(rest));
        };
        CustomLogAppender.prototype.error = function (logger, message) {
            var rest = [];
            for (var _i = 2; _i < arguments.length; _i++) {
                rest[_i - 2] = arguments[_i];
            }
            console.error.apply(console, ["ERROR [" + logger.id + "] " + message].concat(rest));
        };
        return CustomLogAppender;
    }());
    exports.CustomLogAppender = CustomLogAppender;
});

define('main',["require", "exports", "aurelia-framework", "./environment", "./core/custom-log-appender"], function (require, exports, aurelia_framework_1, environment_1, custom_log_appender_1) {
    "use strict";
    Promise.config({
        warnings: {
            wForgottenReturn: false
        }
    });
    aurelia_framework_1.LogManager.addAppender(new custom_log_appender_1.CustomLogAppender());
    aurelia_framework_1.LogManager.setLevel(aurelia_framework_1.LogManager.logLevel.debug);
    function configure(aurelia) {
        aurelia.use
            .standardConfiguration()
            .feature('resources');
        aurelia_framework_1.ViewLocator.prototype.convertOriginToViewUrl = function (origin) {
            var moduleId = origin.moduleId;
            var isVm = moduleId.endsWith('.view-model');
            var id = isVm ? moduleId.split('/')[1].split('.')[0] : moduleId;
            return isVm ? "views/" + id + ".view.html" : id + ".html";
        };
        if (environment_1.default.testing) {
            aurelia.use.plugin('aurelia-testing');
        }
        aurelia.start().then(function () { return aurelia.setRoot(); });
    }
    exports.configure = configure;
});

define('core/mdl-helper',["require", "exports"], function (require, exports) {
    "use strict";
    var MdlHelper = (function () {
        function MdlHelper() {
        }
        MdlHelper.checkMdlComponents = function (element) {
            if (!element) {
                return;
            }
            this.checkTextfield(element);
            this.checkCheckbox(element);
        };
        MdlHelper.checkTextfield = function (element) {
            var inputs = element.querySelectorAll('.mdl-js-textfield');
            var count = inputs.length;
            for (var i = 0; i < count; i++) {
                var input = inputs[i];
                if (!input) {
                    return;
                }
                input.MaterialTextfield.checkDirty();
                input.MaterialTextfield.checkValidity();
            }
        };
        MdlHelper.checkCheckbox = function (element) {
            var inputs = element.querySelectorAll('.mdl-js-checkbox');
            var count = inputs.length;
            for (var i = 0; i < count; i++) {
                var input = inputs[i];
                if (!input) {
                    return;
                }
                input.MaterialCheckbox.checkToggleState();
            }
        };
        return MdlHelper;
    }());
    exports.MdlHelper = MdlHelper;
});

define('core/base-dialog',["require", "exports", "dialog-polyfill/dialog-polyfill", "./mdl-helper"], function (require, exports, dialogPolyfill, mdl_helper_1) {
    "use strict";
    var BaseDialog = (function () {
        function BaseDialog(element, dialogId) {
            this.element = element;
            this.dialogId = dialogId;
        }
        BaseDialog.prototype.register = function () {
            this.dialog = document.querySelector("#" + this.dialogId);
            if (this.dialog.showModal) {
                return;
            }
            dialogPolyfill.registerDialog(this.dialog);
        };
        BaseDialog.prototype.showModal = function () {
            var _this = this;
            this.dialog.showModal();
            setTimeout(function () {
                mdl_helper_1.MdlHelper.checkMdlComponents(_this.element);
            });
        };
        BaseDialog.prototype.dismiss = function (args) {
            var dismissEvent;
            if (window.CustomEvent) {
                dismissEvent = new CustomEvent('dismiss', {
                    detail: {
                        args: args,
                    }, bubbles: true
                });
            }
            else {
                dismissEvent = document.createEvent('CustomEvent');
                dismissEvent.initCustomEvent('dismiss', true, true, {
                    detail: {
                        args: args
                    }
                });
            }
            this.element.dispatchEvent(dismissEvent);
            this.dialog.close();
        };
        BaseDialog.prototype.cancel = function () {
            this.dialog.close();
        };
        return BaseDialog;
    }());
    exports.BaseDialog = BaseDialog;
});

define('core/base-view-model',["require", "exports", "./mdl-helper"], function (require, exports, mdl_helper_1) {
    "use strict";
    var BaseViewModel = (function () {
        function BaseViewModel(viewId) {
            this.viewId = viewId;
            this.loadingText = 'Loading...';
        }
        BaseViewModel.prototype.initMdl = function () {
            var element = document.querySelector("[membertrack-view=\"" + this.viewId + "\"]");
            setTimeout(function () {
                mdl_helper_1.MdlHelper.checkMdlComponents(element);
            });
        };
        BaseViewModel.prototype.updateLoadingText = function (count) {
            this.loadingText = count > 0 ? '' : 'No items available.';
        };
        return BaseViewModel;
    }());
    exports.BaseViewModel = BaseViewModel;
});

define('resources/index',["require", "exports"], function (require, exports) {
    "use strict";
    function configure(config) {
        config.globalResources([
            './attributes/toggle-drawer',
            './attributes/mdl',
            './value-converters/sort',
            './value-converters/group',
            './value-converters/filter',
            './value-converters/date-format',
            './elements/snackbar'
        ]);
    }
    exports.configure = configure;
});

var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('services/address.service',["require", "exports", "aurelia-framework", "aurelia-http-client", "aurelia-event-aggregator", "./base.service"], function (require, exports, aurelia_framework_1, aurelia_http_client_1, aurelia_event_aggregator_1, base_service_1) {
    "use strict";
    var AddressService = (function (_super) {
        __extends(AddressService, _super);
        function AddressService(client, eventAggregator) {
            return _super.call(this, client, aurelia_framework_1.LogManager.getLogger('AddressService'), eventAggregator) || this;
        }
        AddressService.prototype.insertOrUpdate = function (id, dto) {
            return this.client.post("address/" + id, dto)
                .then(function (result) { return JSON.parse(result.response); })
                .catch(this.handleError);
        };
        AddressService.prototype.remove = function (id) {
            return this.client.delete("address/" + id)
                .then(function (result) {
                return true;
            })
                .catch(this.handleError);
        };
        return AddressService;
    }(base_service_1.BaseService));
    AddressService = __decorate([
        aurelia_framework_1.autoinject,
        __metadata("design:paramtypes", [aurelia_http_client_1.HttpClient, aurelia_event_aggregator_1.EventAggregator])
    ], AddressService);
    exports.AddressService = AddressService;
});

var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('services/lookup.service',["require", "exports", "aurelia-framework", "aurelia-http-client", "aurelia-event-aggregator", "./base.service"], function (require, exports, aurelia_framework_1, aurelia_http_client_1, aurelia_event_aggregator_1, base_service_1) {
    "use strict";
    var LookupService = (function (_super) {
        __extends(LookupService, _super);
        function LookupService(client, eventAggregator) {
            return _super.call(this, client, aurelia_framework_1.LogManager.getLogger('LookupService'), eventAggregator) || this;
        }
        LookupService.prototype.getAll = function () {
            if (!this.lookupResultPromise) {
                this.lookupResultPromise = this.client.get('lookup')
                    .then(function (result) { return JSON.parse(result.response); })
                    .catch(this.handleError);
            }
            return this.lookupResultPromise;
        };
        return LookupService;
    }(base_service_1.BaseService));
    LookupService = __decorate([
        aurelia_framework_1.autoinject,
        __metadata("design:paramtypes", [aurelia_http_client_1.HttpClient, aurelia_event_aggregator_1.EventAggregator])
    ], LookupService);
    exports.LookupService = LookupService;
});

var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('services/person.service',["require", "exports", "aurelia-framework", "aurelia-http-client", "aurelia-event-aggregator", "./base.service"], function (require, exports, aurelia_framework_1, aurelia_http_client_1, aurelia_event_aggregator_1, base_service_1) {
    "use strict";
    var PersonService = (function (_super) {
        __extends(PersonService, _super);
        function PersonService(client, eventAggregator) {
            return _super.call(this, client, aurelia_framework_1.LogManager.getLogger('PersonService'), eventAggregator) || this;
        }
        PersonService.prototype.search = function (contains) {
            return this.client.get("person/search?contains=" + contains)
                .then(function (result) { return JSON.parse(result.response); })
                .catch(this.handleError);
        };
        PersonService.prototype.get = function (id) {
            return this.client.get("person/" + id)
                .then(function (result) { return JSON.parse(result.response); })
                .catch(this.handleError);
        };
        PersonService.prototype.insert = function (dto) {
            return this.client.post('person', dto)
                .then(function (result) { return JSON.parse(result.response); })
                .catch(this.handleError);
        };
        PersonService.prototype.update = function (id, dto) {
            return this.client.put("person/" + id, dto)
                .then(function (result) { return JSON.parse(result.response); })
                .catch(this.handleError);
        };
        PersonService.prototype.remove = function (id) {
            return this.client.delete("person/" + id)
                .then(function (result) {
                return true;
            })
                .catch(this.handleError);
        };
        PersonService.prototype.insertOrUpdateChildrenInfo = function (id, dto) {
            return this.client.post("person/childrenInfo/" + id, dto)
                .then(function (result) { return JSON.parse(result.response); })
                .catch(this.handleError);
        };
        PersonService.prototype.insertOrRemoveCheckListItem = function (id, dto) {
            return this.client.post("person/checkListItem/" + id, dto)
                .then(function (result) { return JSON.parse(result.response); })
                .catch(this.handleError);
        };
        return PersonService;
    }(base_service_1.BaseService));
    PersonService = __decorate([
        aurelia_framework_1.autoinject,
        __metadata("design:paramtypes", [aurelia_http_client_1.HttpClient, aurelia_event_aggregator_1.EventAggregator])
    ], PersonService);
    exports.PersonService = PersonService;
});

var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('services/user.service',["require", "exports", "aurelia-framework", "aurelia-http-client", "aurelia-event-aggregator", "./base.service"], function (require, exports, aurelia_framework_1, aurelia_http_client_1, aurelia_event_aggregator_1, base_service_1) {
    "use strict";
    var UserService = (function (_super) {
        __extends(UserService, _super);
        function UserService(client, eventAggregator) {
            return _super.call(this, client, aurelia_framework_1.LogManager.getLogger('UserService'), eventAggregator) || this;
        }
        UserService.prototype.getAll = function () {
            return this.client.get('user')
                .then(function (result) { return JSON.parse(result.response); })
                .catch(this.handleError);
        };
        UserService.prototype.update = function (id, dto) {
            return this.client.put("user/" + id, dto)
                .then(function (result) { return JSON.parse(result.response); })
                .catch(this.handleError);
        };
        UserService.prototype.insert = function (dto) {
            return this.client.post('user', dto)
                .then(function (result) { return JSON.parse(result.response); })
                .catch(this.handleError);
        };
        UserService.prototype.updatePassword = function (id, dto) {
            return this.client.put("user/updatePassword/" + id, dto)
                .then(function (result) { return JSON.parse(result.response); })
                .catch(this.handleError);
        };
        UserService.prototype.remove = function (id) {
            return this.client.delete("user/" + id)
                .then(function (result) {
                return true;
            })
                .catch(this.handleError);
        };
        return UserService;
    }(base_service_1.BaseService));
    UserService = __decorate([
        aurelia_framework_1.autoinject,
        __metadata("design:paramtypes", [aurelia_http_client_1.HttpClient, aurelia_event_aggregator_1.EventAggregator])
    ], UserService);
    exports.UserService = UserService;
});

define('view-models/401.view-model',["require", "exports"], function (require, exports) {
    "use strict";
    var NotAuhorizedViewModel = (function () {
        function NotAuhorizedViewModel() {
        }
        return NotAuhorizedViewModel;
    }());
    exports.NotAuhorizedViewModel = NotAuhorizedViewModel;
});

define('view-models/404.view-model',["require", "exports"], function (require, exports) {
    "use strict";
    var NotFoundViewModel = (function () {
        function NotFoundViewModel() {
        }
        return NotFoundViewModel;
    }());
    exports.NotFoundViewModel = NotFoundViewModel;
});

var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('view-models/address-dialog.view-model',["require", "exports", "aurelia-framework", "../core/base-dialog", "../services/address.service", "../services/lookup.service", "../core/custom-events"], function (require, exports, aurelia_framework_1, base_dialog_1, address_service_1, lookup_service_1, custom_events_1) {
    "use strict";
    var AddressDialogViewModel = (function (_super) {
        __extends(AddressDialogViewModel, _super);
        function AddressDialogViewModel(element, addressService, lookupService) {
            var _this = _super.call(this, element, 'address-dialog') || this;
            _this.addressService = addressService;
            _this.lookupService = lookupService;
            _this.model = {};
            _this.states = [];
            return _this;
        }
        AddressDialogViewModel.prototype.attached = function () {
            var _this = this;
            this.register();
            this.lookupService.getAll().then(function (result) {
                if (!result) {
                    return;
                }
                _this.states = result.states;
            });
        };
        AddressDialogViewModel.prototype.show = function (memberId, model) {
            this.memberId = memberId;
            if (model) {
                this.model = model;
            }
            this.showModal();
        };
        AddressDialogViewModel.prototype.save = function () {
            var _this = this;
            if (!this.memberId) {
                return;
            }
            if (!this.model.city || !this.model.state ||
                !this.model.street || !this.model.zipCode) {
                return;
            }
            this.addressService.insertOrUpdate(this.memberId, this.model).then(function (dto) {
                if (!dto) {
                    return;
                }
                _this.dismiss(new custom_events_1.PersonEvent(dto));
            });
        };
        return AddressDialogViewModel;
    }(base_dialog_1.BaseDialog));
    AddressDialogViewModel = __decorate([
        aurelia_framework_1.customElement('membertrack-address-dialog'),
        __metadata("design:paramtypes", [Element, address_service_1.AddressService, lookup_service_1.LookupService])
    ], AddressDialogViewModel);
    exports.AddressDialogViewModel = AddressDialogViewModel;
});

var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('view-models/check-list-item-dialog.view-model',["require", "exports", "aurelia-framework", "../core/base-dialog", "../services/person.service", "../core/custom-events"], function (require, exports, aurelia_framework_1, base_dialog_1, person_service_1, custom_events_1) {
    "use strict";
    var CheckListItemDialogViewModel = (function (_super) {
        __extends(CheckListItemDialogViewModel, _super);
        function CheckListItemDialogViewModel(element, personService) {
            var _this = _super.call(this, element, 'check-list-item-dialog') || this;
            _this.personService = personService;
            _this.model = {};
            return _this;
        }
        CheckListItemDialogViewModel.prototype.attached = function () {
            this.register();
        };
        CheckListItemDialogViewModel.prototype.show = function (memberId, model) {
            this.memberId = memberId;
            this.model = model;
            this.showModal();
        };
        CheckListItemDialogViewModel.prototype.save = function () {
            var _this = this;
            if (!this.memberId || !this.model.id) {
                return;
            }
            this.personService.insertOrRemoveCheckListItem(this.memberId, this.model).then(function (dto) {
                if (!dto) {
                    return;
                }
                _this.dismiss(new custom_events_1.PersonEvent(dto));
            });
        };
        return CheckListItemDialogViewModel;
    }(base_dialog_1.BaseDialog));
    CheckListItemDialogViewModel = __decorate([
        aurelia_framework_1.customElement('membertrack-check-list-item-dialog'),
        __metadata("design:paramtypes", [Element, person_service_1.PersonService])
    ], CheckListItemDialogViewModel);
    exports.CheckListItemDialogViewModel = CheckListItemDialogViewModel;
});

var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('view-models/children-info-dialog.view-model',["require", "exports", "aurelia-framework", "../core/base-dialog", "../services/person.service", "../core/custom-events"], function (require, exports, aurelia_framework_1, base_dialog_1, person_service_1, custom_events_1) {
    "use strict";
    var ChildrenInfoDialogViewModel = (function (_super) {
        __extends(ChildrenInfoDialogViewModel, _super);
        function ChildrenInfoDialogViewModel(element, personService) {
            var _this = _super.call(this, element, 'children-info-dialog') || this;
            _this.personService = personService;
            _this.model = {};
            return _this;
        }
        ChildrenInfoDialogViewModel.prototype.attached = function () {
            this.register();
        };
        ChildrenInfoDialogViewModel.prototype.show = function (memberId, model) {
            this.memberId = memberId;
            if (model) {
                this.model = model;
            }
            this.showModal();
        };
        ChildrenInfoDialogViewModel.prototype.save = function () {
            var _this = this;
            if (!this.memberId) {
                return;
            }
            this.personService.insertOrUpdateChildrenInfo(this.memberId, this.model).then(function (dto) {
                if (!dto) {
                    return;
                }
                _this.dismiss(new custom_events_1.PersonEvent(dto));
            });
        };
        return ChildrenInfoDialogViewModel;
    }(base_dialog_1.BaseDialog));
    ChildrenInfoDialogViewModel = __decorate([
        aurelia_framework_1.customElement('membertrack-children-info-dialog'),
        __metadata("design:paramtypes", [Element, person_service_1.PersonService])
    ], ChildrenInfoDialogViewModel);
    exports.ChildrenInfoDialogViewModel = ChildrenInfoDialogViewModel;
});

var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('view-models/member-dialog.view-model',["require", "exports", "aurelia-framework", "../core/base-dialog", "../services/person.service", "../services/lookup.service", "../core/custom-events"], function (require, exports, aurelia_framework_1, base_dialog_1, person_service_1, lookup_service_1, custom_events_1) {
    "use strict";
    var MemberDialogViewModel = (function (_super) {
        __extends(MemberDialogViewModel, _super);
        function MemberDialogViewModel(element, personService, lookupService) {
            var _this = _super.call(this, element, 'member-dialog') || this;
            _this.personService = personService;
            _this.lookupService = lookupService;
            _this.model = {};
            _this.genders = [];
            _this.ageGroups = [];
            _this.statusList = [];
            return _this;
        }
        MemberDialogViewModel.prototype.attached = function () {
            var _this = this;
            this.register();
            this.lookupService.getAll().then(function (result) {
                if (!result) {
                    return;
                }
                _this.genders = result.genders;
                _this.ageGroups = result.ageGroups;
                _this.statusList = result.personStatus;
            });
        };
        MemberDialogViewModel.prototype.show = function (model) {
            if (model) {
                this.model = model;
            }
            this.showModal();
        };
        MemberDialogViewModel.prototype.save = function () {
            var _this = this;
            if (!this.model.firstName || !this.model.lastName ||
                !this.model.gender || !this.model.status) {
                return;
            }
            var func = this.model.id ?
                this.personService.update(this.model.id, this.model) :
                this.personService.insert(this.model);
            func.then(function (dto) {
                if (!dto) {
                    return;
                }
                _this.dismiss(new custom_events_1.PersonEvent(dto));
            });
        };
        return MemberDialogViewModel;
    }(base_dialog_1.BaseDialog));
    MemberDialogViewModel = __decorate([
        aurelia_framework_1.customElement('membertrack-member-dialog'),
        __metadata("design:paramtypes", [Element, person_service_1.PersonService, lookup_service_1.LookupService])
    ], MemberDialogViewModel);
    exports.MemberDialogViewModel = MemberDialogViewModel;
});

var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('view-models/home.view-model',["require", "exports", "aurelia-framework", "aurelia-router", "../services/person.service"], function (require, exports, aurelia_framework_1, aurelia_router_1, person_service_1) {
    "use strict";
    var HomeViewModel = (function () {
        function HomeViewModel(personService, router) {
            this.personService = personService;
            this.router = router;
        }
        HomeViewModel.prototype.displayMemberDialog = function () {
            this.memberDialogVm.show();
        };
        HomeViewModel.prototype.dismissMemberDialog = function (e) {
            var event = e.detail.args;
            this.router.navigateToRoute('member-details', { id: event.data.id });
        };
        return HomeViewModel;
    }());
    HomeViewModel = __decorate([
        aurelia_framework_1.autoinject,
        __metadata("design:paramtypes", [person_service_1.PersonService, aurelia_router_1.Router])
    ], HomeViewModel);
    exports.HomeViewModel = HomeViewModel;
});

var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('view-models/main-layout.view-model',["require", "exports", "aurelia-framework", "../services/auth.service"], function (require, exports, aurelia_framework_1, auth_service_1) {
    "use strict";
    var MainLayoutViewModel = (function () {
        function MainLayoutViewModel(authService) {
            this.authService = authService;
            this.contextUser = { displayName: 'Loading...' };
        }
        Object.defineProperty(MainLayoutViewModel.prototype, "viewTitle", {
            get: function () {
                return this.router.currentInstruction.config.title;
            },
            enumerable: true,
            configurable: true
        });
        MainLayoutViewModel.prototype.configureRouter = function (config, router) {
            config.map([
                {
                    route: '',
                    redirect: 'main/home'
                },
                {
                    route: 'home',
                    moduleId: 'view-models/home.view-model',
                    name: 'home',
                    title: 'Home',
                    adminView: false,
                    caseSensitive: true
                },
                {
                    route: 'search',
                    moduleId: 'view-models/search.view-model',
                    name: 'search',
                    title: 'Member Search',
                    adminView: false,
                    caseSensitive: true
                },
                {
                    route: 'member-details/:id',
                    moduleId: 'view-models/member-details.view-model',
                    name: 'member-details',
                    title: 'Member Details',
                    adminView: false,
                    caseSensitive: true
                },
                {
                    route: 'administration/users',
                    moduleId: 'view-models/users.view-model',
                    name: 'users',
                    title: 'User Administration',
                    adminView: true,
                    caseSensitive: true
                }
            ]);
            this.router = router;
        };
        MainLayoutViewModel.prototype.attached = function (argument) {
            var _this = this;
            this.authService.getContextUser().then(function (user) {
                if (!user) {
                    return;
                }
                _this.contextUser = user;
            });
        };
        MainLayoutViewModel.prototype.signOut = function (e) {
            this.authService.signOut();
        };
        return MainLayoutViewModel;
    }());
    __decorate([
        aurelia_framework_1.computedFrom('router.currentInstruction'),
        __metadata("design:type", String),
        __metadata("design:paramtypes", [])
    ], MainLayoutViewModel.prototype, "viewTitle", null);
    MainLayoutViewModel = __decorate([
        aurelia_framework_1.autoinject,
        __metadata("design:paramtypes", [auth_service_1.AuthService])
    ], MainLayoutViewModel);
    exports.MainLayoutViewModel = MainLayoutViewModel;
});

var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('view-models/prompt-delete-dialog.view-model',["require", "exports", "aurelia-framework", "../core/base-dialog", "../core/custom-events"], function (require, exports, aurelia_framework_1, base_dialog_1, custom_events_1) {
    "use strict";
    var PromptDeleteDialogViewModel = (function (_super) {
        __extends(PromptDeleteDialogViewModel, _super);
        function PromptDeleteDialogViewModel(element) {
            var _this = _super.call(this, element, 'prompt-delete-dialog') || this;
            _this.model = {};
            _this.title = 'Delete';
            return _this;
        }
        PromptDeleteDialogViewModel.prototype.attached = function () {
            this.register();
        };
        PromptDeleteDialogViewModel.prototype.show = function (id, name, title) {
            if (title) {
                this.title = title;
            }
            this.model = { id: id, name: name };
            this.showModal();
        };
        PromptDeleteDialogViewModel.prototype.ok = function () {
            this.dismiss(new custom_events_1.DeleteItemEvent(this.model.id));
        };
        return PromptDeleteDialogViewModel;
    }(base_dialog_1.BaseDialog));
    PromptDeleteDialogViewModel = __decorate([
        aurelia_framework_1.customElement('membertrack-prompt-delete-dialog'),
        __metadata("design:paramtypes", [Element])
    ], PromptDeleteDialogViewModel);
    exports.PromptDeleteDialogViewModel = PromptDeleteDialogViewModel;
});

var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('view-models/member-details.view-model',["require", "exports", "aurelia-framework", "../core/base-view-model", "../services/person.service", "../services/lookup.service", "../core/dtos"], function (require, exports, aurelia_framework_1, base_view_model_1, person_service_1, lookup_service_1, dtos_1) {
    "use strict";
    var MemberDetailsViewModel = (function (_super) {
        __extends(MemberDetailsViewModel, _super);
        function MemberDetailsViewModel(personService, lookupService, dtoHelper) {
            var _this = _super.call(this, 'member-details') || this;
            _this.personService = personService;
            _this.lookupService = lookupService;
            _this.dtoHelper = dtoHelper;
            _this.initDefaultModel();
            return _this;
        }
        MemberDetailsViewModel.prototype.activate = function (params) {
            var _this = this;
            var id = params.id;
            this.personService.get(id).then(function (dto) {
                if (!dto) {
                    _this.initDefaultModel();
                    return;
                }
                _this.refreshModel(dto);
            });
        };
        MemberDetailsViewModel.prototype.initDefaultModel = function () {
            this.person = {
                address: {},
                childrenInfo: {},
                checkListItems: []
            };
        };
        MemberDetailsViewModel.prototype.displayMemberDialog = function () {
            this.memberDialogVm.show(Object.assign({}, this.person));
        };
        MemberDetailsViewModel.prototype.displayAddressDialog = function () {
            this.addressDialogVm.show(this.person.id, Object.assign({}, this.person.address));
        };
        MemberDetailsViewModel.prototype.displayChildrenInfoDialog = function () {
            this.childrenInfoDialogVm.show(this.person.id, Object.assign({}, this.person.childrenInfo));
        };
        MemberDetailsViewModel.prototype.checkListItemChange = function (e, item) {
            item.isSelected = !item.isSelected;
            this.initMdl();
            if (!item.date) {
                this.checkListItemDialogVm.show(this.person.id, Object.assign({}, item));
                return;
            }
            this.promptDeleteDialogVm.show(item.id, item.description, 'Uncheck');
        };
        MemberDetailsViewModel.prototype.dismissPromptDeleteDialog = function (e) {
            var _this = this;
            var event = e.detail.args;
            var index = this.dtoHelper.getIndexOf(this.person.checkListItems, event.data);
            var model = this.person.checkListItems[index];
            this.personService.insertOrRemoveCheckListItem(this.person.id, model).then(function (dto) {
                if (!dto) {
                    return;
                }
                _this.refreshModel(dto);
            });
        };
        MemberDetailsViewModel.prototype.dismissDialog = function (e) {
            var event = e.detail.args;
            this.refreshModel(event.data);
        };
        MemberDetailsViewModel.prototype.refreshModel = function (dto) {
            this.refreshCheckListItems(dto);
            this.person = dto;
            this.initMdl();
        };
        MemberDetailsViewModel.prototype.refreshCheckListItems = function (dto) {
            dto.checkListItems.forEach(function (item) {
                if (item.date) {
                    item.isSelected = true;
                }
            });
        };
        return MemberDetailsViewModel;
    }(base_view_model_1.BaseViewModel));
    MemberDetailsViewModel = __decorate([
        aurelia_framework_1.autoinject,
        __metadata("design:paramtypes", [person_service_1.PersonService, lookup_service_1.LookupService, dtos_1.DtoHelper])
    ], MemberDetailsViewModel);
    exports.MemberDetailsViewModel = MemberDetailsViewModel;
});

var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('view-models/search.view-model',["require", "exports", "aurelia-framework", "../services/person.service", "../core/base-view-model"], function (require, exports, aurelia_framework_1, person_service_1, base_view_model_1) {
    "use strict";
    var SearchViewModel = (function (_super) {
        __extends(SearchViewModel, _super);
        function SearchViewModel(personService, observerLocator) {
            var _this = _super.call(this, 'search') || this;
            _this.personService = personService;
            _this.observerLocator = observerLocator;
            _this.people = [];
            _this.hasResults = true;
            _this.searchText = '';
            return _this;
        }
        SearchViewModel.prototype.attached = function () {
            var that = this;
            that.subscriber = that.observerLocator.getObserver(that, 'searchText').subscribe(function (newValue) {
                that.personService.search(newValue).then(function (result) {
                    if (!result) {
                        return;
                    }
                    that.people = result.data;
                    that.hasResults = result.data.length > 0;
                });
            });
        };
        SearchViewModel.prototype.detached = function () {
            if (!this.subscriber) {
                return;
            }
            this.subscriber.dispose();
        };
        return SearchViewModel;
    }(base_view_model_1.BaseViewModel));
    SearchViewModel = __decorate([
        aurelia_framework_1.autoinject,
        __metadata("design:paramtypes", [person_service_1.PersonService, aurelia_framework_1.ObserverLocator])
    ], SearchViewModel);
    exports.SearchViewModel = SearchViewModel;
});

var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('view-models/sign-in.view-model',["require", "exports", "aurelia-framework", "aurelia-router", "aurelia-event-aggregator", "../services/auth.service", "../core/custom-events"], function (require, exports, aurelia_framework_1, aurelia_router_1, aurelia_event_aggregator_1, auth_service_1, custom_events_1) {
    "use strict";
    var SignInViewModel = (function () {
        function SignInViewModel(router, authService, eventAggregator) {
            this.model = {};
            this.router = router;
            this.authService = authService;
            this.eventAggregator = eventAggregator;
        }
        SignInViewModel.prototype.signIn = function (e) {
            var _this = this;
            this.authService.signIn(this.model).then(function (ok) {
                if (!ok) {
                    return;
                }
                _this.eventAggregator.publish(new custom_events_1.SnackbarEvent('Welcome =)'));
                _this.router.navigate(_this.authService.redirectUrl || 'main/home');
            });
        };
        return SignInViewModel;
    }());
    SignInViewModel = __decorate([
        aurelia_framework_1.autoinject,
        __metadata("design:paramtypes", [aurelia_router_1.Router, auth_service_1.AuthService, aurelia_event_aggregator_1.EventAggregator])
    ], SignInViewModel);
    exports.SignInViewModel = SignInViewModel;
});

var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('view-models/user-dialog.view-model',["require", "exports", "aurelia-framework", "../core/base-dialog", "../services/user.service", "../services/lookup.service", "../core/custom-events"], function (require, exports, aurelia_framework_1, base_dialog_1, user_service_1, lookup_service_1, custom_events_1) {
    "use strict";
    var UserDialogViewModel = (function (_super) {
        __extends(UserDialogViewModel, _super);
        function UserDialogViewModel(element, userService, lookupService) {
            var _this = _super.call(this, element, 'user-dialog') || this;
            _this.userService = userService;
            _this.lookupService = lookupService;
            _this.model = {};
            _this.roles = [];
            return _this;
        }
        UserDialogViewModel.prototype.attached = function () {
            var _this = this;
            this.register();
            this.lookupService.getAll().then(function (result) {
                if (!result) {
                    return;
                }
                _this.roles = result.roles;
            });
        };
        UserDialogViewModel.prototype.show = function (model) {
            this.model = model;
            this.showModal();
        };
        UserDialogViewModel.prototype.save = function () {
            var _this = this;
            if (!this.model.displayName || !this.model.email || !this.model.role) {
                return;
            }
            var func = this.model.id ?
                this.userService.update(this.model.id, this.model) :
                this.userService.insert(this.model);
            func.then(function (dto) {
                if (!dto) {
                    return;
                }
                _this.dismiss(new custom_events_1.UserEvent(dto));
            });
        };
        return UserDialogViewModel;
    }(base_dialog_1.BaseDialog));
    UserDialogViewModel = __decorate([
        aurelia_framework_1.customElement('membertrack-user-dialog'),
        __metadata("design:paramtypes", [Element, user_service_1.UserService, lookup_service_1.LookupService])
    ], UserDialogViewModel);
    exports.UserDialogViewModel = UserDialogViewModel;
});

var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('view-models/users.view-model',["require", "exports", "aurelia-framework", "../services/user.service", "../core/base-view-model", "../core/dtos"], function (require, exports, aurelia_framework_1, user_service_1, base_view_model_1, dtos_1) {
    "use strict";
    var UsersViewModel = (function (_super) {
        __extends(UsersViewModel, _super);
        function UsersViewModel(userService, dtoHelper) {
            var _this = _super.call(this, 'users') || this;
            _this.userService = userService;
            _this.dtoHelper = dtoHelper;
            _this.users = [];
            return _this;
        }
        UsersViewModel.prototype.attached = function (argument) {
            var _this = this;
            this.userService.getAll().then(function (dtos) {
                if (!dtos) {
                    return;
                }
                _this.users = dtos;
                _this.updateLoadingText(dtos.length);
            });
        };
        UsersViewModel.prototype.displayUserDialog = function (dto) {
            this.userDialogVm.show(Object.assign({}, dto));
        };
        UsersViewModel.prototype.dismissUserDialog = function (e) {
            var event = e.detail.args;
            this.users = this.dtoHelper.insertOrUpdate(this.users, event.data);
            this.updateLoadingText(this.users.length);
        };
        UsersViewModel.prototype.displayPromptDeleteDialog = function (dto) {
            this.promptDeleteDialogVm.show(dto.id, dto.displayName);
        };
        UsersViewModel.prototype.dismissPromptDeleteDialog = function (e) {
            var _this = this;
            var event = e.detail.args;
            this.userService.remove(event.data).then(function (ok) {
                if (!ok) {
                    return;
                }
                _this.dtoHelper.remove(_this.users, event.data);
                _this.updateLoadingText(_this.users.length);
            });
        };
        return UsersViewModel;
    }(base_view_model_1.BaseViewModel));
    UsersViewModel = __decorate([
        aurelia_framework_1.autoinject,
        __metadata("design:paramtypes", [user_service_1.UserService, dtos_1.DtoHelper])
    ], UsersViewModel);
    exports.UsersViewModel = UsersViewModel;
});

var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('resources/attributes/mdl',["require", "exports", "aurelia-framework"], function (require, exports, aurelia_framework_1) {
    "use strict";
    var MdlControls = {
        'button': { type: 'MaterialButton', classes: ['mdl-button', 'mdl-js-button'], rippleEffectSupport: true },
        'textfield': { type: 'MaterialTextfield', classes: ['mdl-textfield', 'mdl-js-textfield'], rippleEffectSupport: false },
        'layout': { type: 'MaterialLayout', classes: ['mdl-layout', 'mdl-js-layout'], rippleEffectSupport: false },
        'menu': { type: 'MaterialMenu', classes: ['mdl-menu', 'mdl-js-menu'], rippleEffectSupport: true },
        'data-table': { type: 'MaterialDataTable', classes: ['mdl-data-table', 'mdl-js-data-table'], rippleEffectSupport: true },
        'tabs': { type: 'MaterialTabs', classes: ['mdl-tabs', 'mdl-js-tabs'], rippleEffectSupport: true },
        'slider': { type: 'MaterialSlider', classes: ['mdl-slider', 'mdl-js-slider'], rippleEffectSupport: false },
        'tooltip': { type: 'MaterialTooltip', classes: ['mdl-tooltip'], rippleEffectSupport: false },
        'progress': { type: 'MaterialProgress', classes: ['mdl-progress', 'mdl-js-progress'], rippleEffectSupport: false },
        'spinner': { type: 'MaterialSpinner', classes: ['mdl-spinner', 'mdl-js-spinner'], rippleEffectSupport: false },
        'badge': { type: 'MaterialBadge', classes: [''], rippleEffectSupport: false },
        'switch': { type: 'MaterialSwitch', classes: ['mdl-switch', 'mdl-js-switch'], rippleEffectSupport: true },
        'radio': { type: 'MaterialRadio', classes: ['mdl-radio', 'mdl-js-radio'], rippleEffectSupport: true },
        'icon-toggle': { type: 'MaterialIconToggle', classes: ['mdl-icon-toggle', 'mdl-js-icon-toggle'], rippleEffectSupport: true },
        'checkbox': { type: 'MaterialCheckbox', classes: ['mdl-checkbox', 'mdl-js-checkbox'], rippleEffectSupport: true },
        'snackbar': { type: 'MaterialSnackbar', classes: ['mdl-snackbar', 'mdl-js-snackbar'], rippleEffectSupport: false },
        'selectfield': { type: 'MaterialSelectfield', classes: ['mdl-selectfield', 'mdl-js-selectfield'], rippleEffectSupport: false }
    };
    var MdlAttribute = (function () {
        function MdlAttribute(element) {
            this.element = element;
        }
        MdlAttribute.prototype.attached = function () {
            if (!componentHandler) {
                throw 'componentHandler is not defined. Material library is required.';
            }
            var attribute = this.element.attributes['membertrack-mdl'];
            var typeName = attribute.value;
            var control = MdlControls[typeName];
            if (!control) {
                throw typeName + " materialization not supported.";
            }
            var _this = this;
            control.classes.forEach(function (item) {
                _this.element.classList.add(item);
            });
            componentHandler.upgradeElement(this.element, control.type);
            if (!control.rippleEffectSupport) {
                return;
            }
            var addRippleEffect = this.element.classList.contains('mdl-js-ripple-effect');
            if (addRippleEffect) {
                componentHandler.upgradeElement(this.element, 'MaterialRipple');
            }
            var elements = this.element.getElementsByClassName('.mdl-js-ripple-effect');
            for (var _i = 0, elements_1 = elements; _i < elements_1.length; _i++) {
                var item = elements_1[_i];
                componentHandler.upgradeElement(item, 'MaterialRipple');
            }
        };
        return MdlAttribute;
    }());
    MdlAttribute = __decorate([
        aurelia_framework_1.customAttribute('membertrack-mdl'),
        aurelia_framework_1.autoinject,
        __metadata("design:paramtypes", [Element])
    ], MdlAttribute);
    exports.MdlAttribute = MdlAttribute;
});

var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('resources/attributes/toggle-drawer',["require", "exports", "aurelia-framework"], function (require, exports, aurelia_framework_1) {
    "use strict";
    var ToggleDrawerAttribute = (function () {
        function ToggleDrawerAttribute(element) {
            this.element = element;
        }
        ToggleDrawerAttribute.prototype.attached = function () {
            this.element.addEventListener('click', this.toggleDrawer);
        };
        ToggleDrawerAttribute.prototype.detached = function () {
            this.element.removeEventListener('click', this.toggleDrawer);
        };
        ToggleDrawerAttribute.prototype.toggleDrawer = function (e) {
            var layout = document.querySelector('.mdl-layout');
            var shouldToggle = layout.classList.contains('is-small-screen');
            if (shouldToggle) {
                layout.MaterialLayout.toggleDrawer();
            }
            e.stopPropagation();
        };
        return ToggleDrawerAttribute;
    }());
    ToggleDrawerAttribute = __decorate([
        aurelia_framework_1.customAttribute('membertrack-toggle-drawer'),
        aurelia_framework_1.autoinject,
        __metadata("design:paramtypes", [Element])
    ], ToggleDrawerAttribute);
    exports.ToggleDrawerAttribute = ToggleDrawerAttribute;
});

var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('resources/elements/snackbar',["require", "exports", "aurelia-framework", "aurelia-event-aggregator", "../../core/custom-events"], function (require, exports, aurelia_framework_1, aurelia_event_aggregator_1, custom_events_1) {
    "use strict";
    var Snackbar = (function () {
        function Snackbar(element, eventAggregator) {
            this.element = element;
            this.eventAggregator = eventAggregator;
        }
        Snackbar.prototype.attached = function () {
            var _this = this;
            this.snackbarContainer = this.element.querySelector('div[membertrack-mdl="snackbar"]');
            this.subscriber = this.eventAggregator.subscribe(custom_events_1.SnackbarEvent, function (e) {
                var data = { message: e.data, timeout: 5000 };
                _this.snackbarContainer.MaterialSnackbar.showSnackbar(data);
            });
        };
        Snackbar.prototype.detached = function () {
            if (!this.subscriber) {
                return;
            }
            this.subscriber.dispose();
        };
        return Snackbar;
    }());
    Snackbar = __decorate([
        aurelia_framework_1.customElement('membertrack-snackbar'),
        __metadata("design:paramtypes", [Element, aurelia_event_aggregator_1.EventAggregator])
    ], Snackbar);
    exports.Snackbar = Snackbar;
});

define('resources/value-converters/date-format',["require", "exports", "moment"], function (require, exports, moment) {
    "use strict";
    var DateFormatValueConverter = (function () {
        function DateFormatValueConverter() {
        }
        DateFormatValueConverter.prototype.toView = function (date, format) {
            return moment(date).format(format);
        };
        return DateFormatValueConverter;
    }());
    exports.DateFormatValueConverter = DateFormatValueConverter;
});

define('resources/value-converters/filter',["require", "exports"], function (require, exports) {
    "use strict";
    var FilterValueConverter = (function () {
        function FilterValueConverter() {
        }
        FilterValueConverter.prototype.toView = function (array, propertyName, text) {
            if (!array || array.length === 0) {
                return [];
            }
            if (!propertyName) {
                return array;
            }
            if (!text) {
                return array;
            }
            text = text.toLocaleLowerCase();
            return array.filter(function (item) {
                var value = item[propertyName];
                if (!value) {
                    return;
                }
                return value.toLocaleLowerCase().indexOf(text) !== -1;
            });
        };
        return FilterValueConverter;
    }());
    exports.FilterValueConverter = FilterValueConverter;
});

define('resources/value-converters/group',["require", "exports"], function (require, exports) {
    "use strict";
    var GroupValueConverter = (function () {
        function GroupValueConverter() {
        }
        GroupValueConverter.prototype.toView = function (array, propertyName) {
            if (!array || array.length === 0) {
                return [];
            }
            if (!propertyName) {
                return array;
            }
            var groups = {};
            array.forEach(function (item) {
                var group = item[propertyName];
                groups[group] = groups[group] || [];
                groups[group].push(item);
            });
            return Object.keys(groups).map(function (group) {
                return {
                    groupName: group,
                    items: groups[group]
                };
            });
        };
        return GroupValueConverter;
    }());
    exports.GroupValueConverter = GroupValueConverter;
});

define('resources/value-converters/sort',["require", "exports"], function (require, exports) {
    "use strict";
    var SortValueConverter = (function () {
        function SortValueConverter() {
        }
        SortValueConverter.prototype.toView = function (array, propertyName, asc) {
            if (!array || array.length === 0) {
                return [];
            }
            if (!propertyName) {
                return array;
            }
            if (asc === undefined) {
                asc = true;
            }
            var func = function (a, b) {
                var result = 0;
                var condition = asc ? (a[propertyName] > b[propertyName]) : (b[propertyName] > a[propertyName]);
                if (condition) {
                    result = 1;
                }
                condition = asc ? (a[propertyName] < b[propertyName]) : (b[propertyName] < a[propertyName]);
                if (condition) {
                    result = -1;
                }
                return result;
            };
            return array.sort(func);
        };
        return SortValueConverter;
    }());
    exports.SortValueConverter = SortValueConverter;
});

define('text!app.html', ['module'], function(module) { module.exports = "<template><require from=material-design-lite/material.min.css></require><require from=mdl-selectfield/mdl-selectfield.css></require><require from=dialog-polyfill/dialog-polyfill.css></require><require from=./styles/dashboard.css></require><require from=./styles/member-track.css></require><router-view></router-view><membertrack-snackbar></membertrack-snackbar><div membertrack-mdl=spinner loading-indicator class=is-active if.bind=isLoading></div></template>"; });
define('text!views/401.view.html', ['module'], function(module) { module.exports = "<template><div class=\"mdl-grid demo-content\"><div class=mdl-layout-spacer></div><div class=page-redirect-text>You are not authorized to access this page.</div><div class=mdl-layout-spacer></div></div></template>"; });
define('text!styles/dashboard.css', ['module'], function(module) { module.exports = "/**\r\n * Copyright 2015 Google Inc. All Rights Reserved.\r\n *\r\n * Licensed under the Apache License, Version 2.0 (the \"License\");\r\n * you may not use this file except in compliance with the License.\r\n * You may obtain a copy of the License at\r\n *\r\n *      http://www.apache.org/licenses/LICENSE-2.0\r\n *\r\n * Unless required by applicable law or agreed to in writing, software\r\n * distributed under the License is distributed on an \"AS IS\" BASIS,\r\n * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.\r\n * See the License for the specific language governing permissions and\r\n * limitations under the License.\r\n */\nbody,\nhtml {\n  font-family: 'Roboto', 'Helvetica', sans-serif; }\n\n.demo-avatar {\n  width: 48px;\n  height: 48px;\n  border-radius: 24px; }\n\n.demo-layout .demo-header .mdl-textfield {\n  padding: 0;\n  margin-top: 41px; }\n\n.demo-layout .demo-header .mdl-textfield .mdl-textfield__expandable-holder {\n  bottom: 19px; }\n\n.demo-layout .mdl-layout__header .mdl-layout__drawer-button {\n  color: rgba(0, 0, 0, 0.54); }\n\n.mdl-layout__drawer .avatar {\n  margin-bottom: 16px; }\n\n.demo-drawer {\n  border: none; }\n\n/* iOS Safari specific workaround */\n.demo-drawer .mdl-menu__container {\n  z-index: -1; }\n\n.demo-drawer .demo-navigation {\n  z-index: -2; }\n\n/* END iOS Safari specific workaround */\n.demo-drawer .mdl-menu .mdl-menu__item {\n  display: -webkit-flex;\n  display: -ms-flexbox;\n  display: flex;\n  -webkit-align-items: center;\n  -ms-flex-align: center;\n  align-items: center; }\n\n.demo-drawer-header {\n  box-sizing: border-box;\n  display: -webkit-flex;\n  display: -ms-flexbox;\n  display: flex;\n  -webkit-flex-direction: column;\n  -ms-flex-direction: column;\n  flex-direction: column;\n  -webkit-justify-content: flex-end;\n  -ms-flex-pack: end;\n  justify-content: flex-end;\n  padding: 16px;\n  height: 151px; }\n\n.demo-avatar-dropdown {\n  display: -webkit-flex;\n  display: -ms-flexbox;\n  display: flex;\n  position: relative;\n  -webkit-flex-direction: row;\n  -ms-flex-direction: row;\n  flex-direction: row;\n  -webkit-align-items: center;\n  -ms-flex-align: center;\n  align-items: center;\n  width: 100%; }\n\n.demo-navigation {\n  -webkit-flex-grow: 1;\n  -ms-flex-positive: 1;\n  flex-grow: 1; }\n\n.demo-layout .demo-navigation .mdl-navigation__link {\n  display: -webkit-flex !important;\n  display: -ms-flexbox !important;\n  display: flex !important;\n  -webkit-flex-direction: row;\n  -ms-flex-direction: row;\n  flex-direction: row;\n  -webkit-align-items: center;\n  -ms-flex-align: center;\n  align-items: center;\n  color: rgba(255, 255, 255, 0.56);\n  font-weight: 500; }\n\n.demo-layout .demo-navigation .mdl-navigation__link:hover {\n  background-color: #00BCD4;\n  color: #37474F; }\n\n.demo-navigation .mdl-navigation__link .material-icons {\n  font-size: 24px;\n  color: rgba(255, 255, 255, 0.56);\n  margin-right: 32px; }\n\n.demo-content {\n  max-width: 1080px; }\n\n.demo-charts {\n  -webkit-align-items: center;\n  -ms-flex-align: center;\n  -ms-grid-row-align: center;\n  align-items: center; }\n\n.demo-chart:nth-child(1) {\n  color: #ACEC00; }\n\n.demo-chart:nth-child(2) {\n  color: #00BBD6; }\n\n.demo-chart:nth-child(3) {\n  color: #BA65C9; }\n\n.demo-chart:nth-child(4) {\n  color: #EF3C79; }\n\n.demo-graphs {\n  padding: 16px 32px;\n  display: -webkit-flex;\n  display: -ms-flexbox;\n  display: flex;\n  -webkit-flex-direction: column;\n  -ms-flex-direction: column;\n  flex-direction: column;\n  -webkit-align-items: stretch;\n  -ms-flex-align: stretch;\n  align-items: stretch; }\n\n/* TODO: Find a proper solution to have the graphs\r\n * not float around outside their container in IE10/11.\r\n * Using a browserhacks.com solution for now.\r\n */\n:root .demo-graphs,\n_:-ms-input-placeholder {\n  min-height: 664px; }\n\n:root .demo-graph,\n_:-ms-input-placeholder {\n  max-height: 300px; }\n\n/* TODO end */\n.demo-graph:nth-child(1) {\n  color: #00b9d8; }\n\n.demo-graph:nth-child(2) {\n  color: #d9006e; }\n\n.demo-cards {\n  -webkit-align-items: flex-start;\n  -ms-flex-align: start;\n  -ms-grid-row-align: flex-start;\n  align-items: flex-start;\n  -webkit-align-content: flex-start;\n  -ms-flex-line-pack: start;\n  align-content: flex-start; }\n\n.demo-cards .demo-separator {\n  height: 32px; }\n\n.demo-cards .mdl-card__title.mdl-card__title {\n  color: white;\n  font-size: 24px;\n  font-weight: 400; }\n\n.demo-cards ul {\n  padding: 0; }\n\n.demo-cards h3 {\n  font-size: 1em; }\n\n.demo-updates .mdl-card__title {\n  min-height: 200px;\n  background-position: 90% 100%;\n  background-repeat: no-repeat; }\n\n.demo-cards .mdl-card__actions a {\n  color: #00BCD4;\n  text-decoration: none; }\n\n.demo-options h3 {\n  margin: 0; }\n\n.demo-options .mdl-checkbox__box-outline {\n  border-color: rgba(255, 255, 255, 0.89); }\n\n.demo-options ul {\n  margin: 0;\n  list-style-type: none; }\n\n.demo-options li {\n  margin: 4px 0; }\n\n.demo-options .material-icons {\n  color: rgba(255, 255, 255, 0.89); }\n\n.demo-options .mdl-card__actions {\n  height: 64px;\n  display: -webkit-flex;\n  display: -ms-flexbox;\n  display: flex;\n  box-sizing: border-box;\n  -webkit-align-items: center;\n  -ms-flex-align: center;\n  align-items: center; }\n"; });
define('text!styles/member-track.css', ['module'], function(module) { module.exports = "/* animations */\n@-webkit-keyframes fadeIn {\n  0% {\n    opacity: 0; }\n  100% {\n    opacity: 1; } }\n\n@-moz-keyframes fadeIn {\n  0% {\n    opacity: 0; }\n  100% {\n    opacity: 1; } }\n\n@-ms-keyframes fadeIn {\n  0% {\n    opacity: 0; }\n  100% {\n    opacity: 1; } }\n\n@keyframes fadeIn {\n  0% {\n    opacity: 0; }\n  100% {\n    opacity: 1; } }\n\nsection {\n  -webkit-animation: fadeIn 1s;\n  animation: fadeIn 1s; }\n\n/* Helper classes */\n.full-width {\n  width: 100%; }\n\ndialog {\n  position: fixed;\n  top: 5%;\n  width: 340px !important; }\n\n#prompt-delete-dialog .mdl-dialog__content p span {\n  font-weight: bolder;\n  padding: 0 5px; }\n\n#app-title {\n  text-align: center;\n  font-size: xx-large;\n  margin-bottom: 30px; }\n\n#current-user {\n  text-align: center; }\n\n#sign-in-btn {\n  width: 100%;\n  padding: 0; }\n\n.check-list-items {\n  margin-bottom: 5px; }\n  .check-list-items span.check-list-item-date {\n    font-style: italic;\n    font-size: small; }\n\n.check-list-item-note {\n  padding-left: 25px; }\n  .check-list-item-note p {\n    font-style: italic; }\n\n.children-info {\n  margin-right: 5px; }\n\n#children-info-dialog label[membertrack-mdl=\"checkbox\"] {\n  margin-bottom: 5px; }\n\n.page-redirect-text, .loading-text {\n  font-size: 30px;\n  line-height: 72px;\n  text-shadow: rgba(0, 0, 0, 0.5) 0 0 15px;\n  text-transform: uppercase;\n  font-family: \"Helvetica Neue\", Helvetica, Arial, sans-serif; }\n\n.loading-text {\n  font-size: 20px;\n  text-align: center; }\n\n/* MDL overrides */\ntd.align {\n  vertical-align: middle !important; }\n\n.mdl-data-table th {\n  text-align: left; }\n\n.mdl-data-table td {\n  text-align: left; }\n\n.mdl-card__title-text {\n  font-size: 16px; }\n\n.mdl-card__supporting-text {\n  width: initial; }\n\n.float-btn, .add-btn {\n  position: fixed;\n  display: block;\n  right: 0;\n  bottom: 0;\n  z-index: 900; }\n\n.add-btn {\n  margin-right: 40px;\n  margin-bottom: 40px; }\n\n[loading-indicator] {\n  position: absolute;\n  width: 100px;\n  height: 100px;\n  left: 70px;\n  bottom: 50px;\n  z-index: 999; }\n"; });
define('text!views/404.view.html', ['module'], function(module) { module.exports = "<template><div class=\"mdl-grid demo-content\"><div class=mdl-layout-spacer></div><div class=page-redirect-text>The page you request is not found.</div><div class=mdl-layout-spacer></div></div></template>"; });
define('text!views/address-dialog.view.html', ['module'], function(module) { module.exports = "<template><dialog id=address-dialog class=mdl-dialog><form role=form submit.delegate=save($event)><h6>${memberId ? 'Edit' : 'Add New'} Address</h6><hr><div class=mdl-dialog__content><div membertrack-mdl=textfield class=\"mdl-textfield--floating-label full-width\"><input class=mdl-textfield__input type=text id=streetControl name=streetControl value.bind=model.street required><label class=mdl-textfield__label for=streetControl>Street...</label></div><div membertrack-mdl=textfield class=\"mdl-textfield--floating-label full-width\"><input class=mdl-textfield__input type=text id=cityControl name=cityControl value.bind=model.city><label class=mdl-textfield__label for=cityControl>City...</label></div><div membertrack-mdl=selectfield class=\"mdl-selectfield--floating-label full-width\"><select id=stateControl name=stateControl class=mdl-selectfield__select value.bind=model.state required><option model.bind=null>Select state...</option><option repeat.for=\"state of states | sort : 'name'\" model.bind=state.id>${state.name}</option></select><label class=mdl-selectfield__label for=stateControl>State...</label></div><div membertrack-mdl=textfield class=\"mdl-textfield--floating-label full-width\"><input class=mdl-textfield__input type=text id=zipCodeControl name=zipCodeControl value.bind=model.zipCode required><label class=mdl-textfield__label for=zipCodeControl>Zip Code...</label></div></div><hr><div class=mdl-dialog__actions><button membertrack-mdl=button type=button click.trigger=cancel()>Cancel</button><button membertrack-mdl=button type=submit class=mdl-button--accent>Save</button></div></form></dialog></template>"; });
define('text!views/check-list-item-dialog.view.html', ['module'], function(module) { module.exports = "<template><dialog id=check-list-item-dialog class=mdl-dialog><form role=form submit.delegate=save($event)><h6>Check '${model.description}''</h6><hr><div class=mdl-dialog__content><div membertrack-mdl=textfield class=\"mdl-textfield--floating-label full-width\"><textarea class=mdl-textfield__input type=text rows=3 id=noteControl value.bind=model.note></textarea><label class=mdl-textfield__label for=noteControl>Add a note...</label></div></div><hr><div class=mdl-dialog__actions><button membertrack-mdl=button type=button click.trigger=cancel()>Cancel</button><button membertrack-mdl=button type=submit class=mdl-button--accent>Save</button></div></form></dialog></template>"; });
define('text!views/children-info-dialog.view.html', ['module'], function(module) { module.exports = "<template><dialog id=children-info-dialog class=mdl-dialog><form role=form submit.delegate=save($event)><h6>Edit Children's' Age Group</h6><hr><div class=mdl-dialog__content><label membertrack-mdl=checkbox class=mdl-js-ripple-effect for=infantControl><input type=checkbox id=infantControl class=mdl-checkbox__input checked.bind=model.hasInfantKids><span class=mdl-checkbox__label>Infant</span></label><label membertrack-mdl=checkbox class=mdl-js-ripple-effect for=toddlerControl><input type=checkbox id=toddlerControl class=mdl-checkbox__input checked.bind=model.hasToddlerKids><span class=mdl-checkbox__label>Toddler</span></label><label membertrack-mdl=checkbox class=mdl-js-ripple-effect for=elementaryControl><input type=checkbox id=elementaryControl class=mdl-checkbox__input checked.bind=model.hasElementaryKids><span class=mdl-checkbox__label>Elementary</span></label><label membertrack-mdl=checkbox class=mdl-js-ripple-effect for=juniorHighControl><input type=checkbox id=juniorHighControl class=mdl-checkbox__input checked.bind=model.hasJuniorHighKids><span class=mdl-checkbox__label>Junior High</span></label><label membertrack-mdl=checkbox class=mdl-js-ripple-effect for=highSchoolControl><input type=checkbox id=highSchoolControl class=mdl-checkbox__input checked.bind=model.hasHighSchoolKids><span class=mdl-checkbox__label>High School</span></label></div><hr><div class=mdl-dialog__actions><button membertrack-mdl=button type=button click.trigger=cancel()>Cancel</button><button membertrack-mdl=button type=submit class=mdl-button--accent>Save</button></div></form></dialog></template>"; });
define('text!views/home.view.html', ['module'], function(module) { module.exports = "<template><require from=../view-models/member-dialog.view-model></require><membertrack-member-dialog view-model.ref=memberDialogVm dismiss.delegate=dismissMemberDialog($event)></membertrack-member-dialog><section><button membertrack-mdl=button type=button class=\"mdl-button--raised mdl-js-ripple-effect mdl-button--primary add-btn\" click.trigger=displayMemberDialog()>New Member</button></section></template>"; });
define('text!views/main-layout.view.html', ['module'], function(module) { module.exports = "<template><div membertrack-mdl=layout class=\"demo-layout mdl-layout--fixed-drawer mdl-layout--fixed-header\"><header class=\"demo-header mdl-layout__header mdl-color--grey-100 mdl-color-text--grey-600\"><div class=mdl-layout__header-row><span class=mdl-layout-title>${viewTitle}</span><div class=mdl-layout-spacer></div><button membertrack-mdl=button class=\"mdl-js-ripple-effect mdl-button--icon\" id=hdrbtn><i class=material-icons>more_vert</i></button><ul membertrack-mdl=menu class=\"mdl-js-ripple-effect mdl-menu--bottom-right\" for=hdrbtn><li class=mdl-menu__item>Change Password</li><li class=mdl-menu__item--full-bleed-divider></li><li class=mdl-menu__item click.trigger=signOut($event)>Sign Out</li></ul></div></header><div class=\"demo-drawer mdl-layout__drawer mdl-color--blue-grey-900 mdl-color-text--blue-grey-50\"><header class=demo-drawer-header><div id=app-title>MemberTrack</div><div id=current-user><span>${contextUser.displayName}</span></div></header><nav class=\"demo-navigation mdl-navigation mdl-color--blue-grey-800\"><a class=mdl-navigation__link membertrack-toggle-drawer href=#main/home><i class=\"mdl-color-text--blue-grey-400 material-icons\" role=presentation>home</i>Home</a><a class=mdl-navigation__link membertrack-toggle-drawer href=#main/search><i class=\"mdl-color-text--blue-grey-400 material-icons\" role=presentation>search</i>Search</a><a class=mdl-navigation__link membertrack-toggle-drawer href=#main/administration/users><i class=\"mdl-color-text--blue-grey-400 material-icons\" role=presentation>people</i>Users</a><div class=mdl-layout-spacer></div></nav></div><main class=\"mdl-layout__content mdl-color--grey-100\"><router-view></router-view></main></div></template>"; });
define('text!views/member-details.view.html', ['module'], function(module) { module.exports = "<template><require from=../view-models/prompt-delete-dialog.view-model></require><require from=../view-models/member-dialog.view-model></require><require from=../view-models/address-dialog.view-model></require><require from=../view-models/children-info-dialog.view-model></require><require from=../view-models/check-list-item-dialog.view-model></require><membertrack-prompt-delete-dialog view-model.ref=promptDeleteDialogVm dismiss.delegate=dismissPromptDeleteDialog($event)></membertrack-prompt-delete-dialog><membertrack-member-dialog view-model.ref=memberDialogVm dismiss.delegate=dismissDialog($event)></membertrack-member-dialog><membertrack-address-dialog view-model.ref=addressDialogVm dismiss.delegate=dismissDialog($event)></membertrack-address-dialog><membertrack-children-info-dialog view-model.ref=childrenInfoDialogVm dismiss.delegate=dismissDialog($event)></membertrack-children-info-dialog><membertrack-check-list-item-dialog view-model.ref=checkListItemDialogVm dismiss.delegate=dismissDialog($event)></membertrack-check-list-item-dialog><section membertrack-view=member-details><div class=\"mdl-grid demo-content\"><div class=\"mdl-card mdl-shadow--2dp full-width\"><div class=mdl-card__supporting-text><div membertrack-mdl=textfield class=\"mdl-textfield--floating-label mdl-cell mdl-cell--6-col\"><input class=mdl-textfield__input type=text id=fnControl value.two-way=person.firstName readonly><label class=mdl-textfield__label for=fnControl>First Name</label></div><div membertrack-mdl=textfield class=\"mdl-textfield--floating-label mdl-cell mdl-cell--6-col\"><input class=mdl-textfield__input type=text id=mnControl value.two-way=person.middleName readonly><label class=mdl-textfield__label for=mnControl>Middle Name</label></div><div membertrack-mdl=textfield class=\"mdl-textfield--floating-label mdl-cell mdl-cell--6-col\"><input class=mdl-textfield__input type=text id=lnControl value.two-way=person.lastName readonly><label class=mdl-textfield__label for=lnControl>Last Name</label></div><div membertrack-mdl=textfield class=\"mdl-textfield--floating-label mdl-cell mdl-cell--6-col\"><input class=mdl-textfield__input type=text id=ageGroupControl value.two-way=person.ageGroupName readonly><label class=mdl-textfield__label for=ageGroupControl>Age Group</label></div><div membertrack-mdl=textfield class=\"mdl-textfield--floating-label mdl-cell mdl-cell--6-col\"><input class=mdl-textfield__input type=text id=genderControl value.two-way=person.genderName readonly><label class=mdl-textfield__label for=genderControl>Gender</label></div><div membertrack-mdl=textfield class=\"mdl-textfield--floating-label mdl-cell mdl-cell--6-col\"><input class=mdl-textfield__input type=text id=statusControl value.two-way=person.statusName readonly><label class=mdl-textfield__label for=statusControl>Status</label></div><div membertrack-mdl=textfield class=\"mdl-textfield--floating-label mdl-cell mdl-cell--6-col\"><input class=mdl-textfield__input type=text id=contactNumberControl value.two-way=person.contactNumber readonly><label class=mdl-textfield__label for=contactNumberControl>Contact #</label></div><div membertrack-mdl=textfield class=\"mdl-textfield--floating-label mdl-cell mdl-cell--6-col\"><input class=mdl-textfield__input type=email id=emailControl value.two-way=person.email readonly><label class=mdl-textfield__label for=emailControl>Email</label></div></div><div class=\"mdl-card__actions mdl-card--border\"><button membertrack-mdl=button class=mdl-button--accent click.trigger=displayMemberDialog()>Edit</button></div></div></div><div class=\"mdl-grid demo-content\"><div class=\"mdl-card mdl-shadow--2dp full-width\"><div class=mdl-card__title><h2 class=mdl-card__title-text>Address</h2></div><div class=mdl-card__supporting-text><div membertrack-mdl=textfield class=\"mdl-textfield--floating-label mdl-cell mdl-cell--6-col\"><input class=mdl-textfield__input type=text id=streetControl value.two-way=person.address.street readonly><label class=mdl-textfield__label for=streetControl>Street</label></div><div membertrack-mdl=textfield class=\"mdl-textfield--floating-label mdl-cell mdl-cell--6-col\"><input class=mdl-textfield__input type=text id=cityControl value.two-way=person.address.city readonly><label class=mdl-textfield__label for=cityControl>City</label></div><div membertrack-mdl=textfield class=\"mdl-textfield--floating-label mdl-cell mdl-cell--6-col\"><input class=mdl-textfield__input type=text id=stateControl value.two-way=person.address.stateName readonly><label class=mdl-textfield__label for=stateControl>State</label></div><div membertrack-mdl=textfield class=\"mdl-textfield--floating-label mdl-cell mdl-cell--6-col\"><input class=mdl-textfield__input type=number id=zipCodeControl value.two-way=person.address.zipCode readonly><label class=mdl-textfield__label for=zipCodeControl>Zip Code</label></div></div><div class=\"mdl-card__actions mdl-card--border\"><button membertrack-mdl=button class=mdl-button--accent click.trigger=displayAddressDialog()>Edit</button></div></div></div><div class=\"mdl-grid demo-content\"><div class=\"mdl-card mdl-shadow--2dp full-width\"><div class=mdl-card__title><h2 class=mdl-card__title-text>Children's Age Group</h2></div><div class=mdl-card__supporting-text><span class=\"mdl-chip children-info\" if.bind=person.childrenInfo.hasInfantKids><span class=mdl-chip__text>Infant</span></span><span class=\"mdl-chip children-info\" if.bind=person.childrenInfo.hasToddlerKids><span class=mdl-chip__text>Toddler</span></span><span class=\"mdl-chip children-info\" if.bind=person.childrenInfo.hasElementaryKids><span class=mdl-chip__text>Elementary</span></span><span class=\"mdl-chip children-info\" if.bind=person.childrenInfo.hasJuniorHighKids><span class=mdl-chip__text>Junior High</span></span><span class=\"mdl-chip children-info\" if.bind=person.childrenInfo.hasHighSchoolKids><span class=mdl-chip__text>High School</span></span><span><br><br><br><br></span></div><div class=\"mdl-card__actions mdl-card--border\"><button membertrack-mdl=button class=mdl-button--accent click.trigger=displayChildrenInfoDialog()>Edit</button></div></div></div><div class=\"mdl-grid demo-content\"><div class=\"mdl-card mdl-shadow--2dp full-width\"><div class=mdl-card__title><h2 class=mdl-card__title-text>Check List</h2></div><div class=mdl-card__supporting-text><div class=check-list-items repeat.for=\"group of person.checkListItems | group : 'typeName' | sort : 'groupName'\"><div><h6>${group.groupName}</h6><div repeat.for=\"item of group.items\"><label membertrack-mdl=checkbox class=mdl-js-ripple-effect for=${item.id}><input type=checkbox id=${item.id} class=mdl-checkbox__input checked.bind=item.isSelected change.delegate=\"checkListItemChange($event, item)\"><span class=mdl-checkbox__label>${item.description} &nbsp;<span class=check-list-item-date if.bind=item.date>(checked on ${item.date | dateFormat : 'MMMM Do YYYY, h:mm:ss a'})</span></span></label><div if.bind=item.note class=check-list-item-note><p>${item.note}</p></div></div></div></div></div></div></div></section></template>"; });
define('text!views/member-dialog.view.html', ['module'], function(module) { module.exports = "<template><dialog id=member-dialog class=mdl-dialog><form role=form submit.delegate=save($event)><h6>${model.id ? 'Edit' : 'Add New'} Member</h6><hr><div class=mdl-dialog__content><div membertrack-mdl=textfield class=\"mdl-textfield--floating-label full-width\"><input class=mdl-textfield__input type=text id=firstNameControl name=firstNameControl value.bind=model.firstName required><label class=mdl-textfield__label for=firstNameControl>First Name...</label></div><div membertrack-mdl=textfield class=\"mdl-textfield--floating-label full-width\"><input class=mdl-textfield__input type=text id=middleNameControl name=middleNameControl value.bind=model.middleName><label class=mdl-textfield__label for=middleNameControl>Middle Name...</label></div><div membertrack-mdl=textfield class=\"mdl-textfield--floating-label full-width\"><input class=mdl-textfield__input type=text id=lastNameControl name=lastNameControl value.bind=model.lastName required><label class=mdl-textfield__label for=lastNameControl>Last Name...</label></div><div membertrack-mdl=textfield class=\"mdl-textfield--floating-label full-width\"><input class=mdl-textfield__input type=email id=emailControl name=emailControl value.bind=model.email><label class=mdl-textfield__label for=emailControl>Email...</label></div><div membertrack-mdl=textfield class=\"mdl-textfield--floating-label full-width\"><input class=mdl-textfield__input type=text id=contactNumberControl name=contactNumberControl value.bind=model.contactNumber><label class=mdl-textfield__label for=contactNumberControl>Contact #...</label></div><div membertrack-mdl=selectfield class=\"mdl-selectfield--floating-label full-width\"><select id=genderControl name=genderControl class=mdl-selectfield__select value.bind=model.gender required><option model.bind=null>Select gender...</option><option repeat.for=\"gender of genders | sort : 'name'\" model.bind=gender.id>${gender.name}</option></select><label class=mdl-selectfield__label for=genderControl>Gender...</label></div><div membertrack-mdl=selectfield class=\"mdl-selectfield--floating-label full-width\"><select id=ageGroupControl name=ageGroupControl class=mdl-selectfield__select value.bind=model.ageGroup><option model.bind=null>Select age group...</option><option repeat.for=\"ageGroup of ageGroups | sort : 'name'\" model.bind=ageGroup.id>${ageGroup.name}</option></select><label class=mdl-selectfield__label for=ageGroupControl>Age Group...</label></div><div membertrack-mdl=selectfield class=\"mdl-selectfield--floating-label full-width\"><select id=statusControl name=statusControl class=mdl-selectfield__select value.bind=model.status required><option model.bind=null>Select status...</option><option repeat.for=\"status of statusList | sort : 'name'\" model.bind=status.id>${status.name}</option></select><label class=mdl-selectfield__label for=statusControl>Status...</label></div></div><hr><div class=mdl-dialog__actions><button membertrack-mdl=button type=button click.trigger=cancel()>Cancel</button><button membertrack-mdl=button type=submit class=mdl-button--accent>Save</button></div></form></dialog></template>"; });
define('text!views/prompt-delete-dialog.view.html', ['module'], function(module) { module.exports = "<template><dialog id=prompt-delete-dialog class=mdl-dialog><h6>${title} ${model.name}</h6><hr><div class=mdl-dialog__content><p>Are you sure you want to<span>${title}</span>${model.name}?</p></div><hr><div class=mdl-dialog__actions><button membertrack-mdl=button type=button click.trigger=cancel()>No</button><button membertrack-mdl=button type=button class=mdl-button--accent click.trigger=ok()>Yes</button></div></dialog></template>"; });
define('text!views/search.view.html', ['module'], function(module) { module.exports = "<template><section membertrack-view=search><div class=\"mdl-grid demo-content\"><div class=mdl-layout-spacer></div><div membertrack-mdl=textfield class=mdl-textfield--floating-label><input class=mdl-textfield__input type=text id=searchControl name=searchControl value.two-way=\"searchText & debounce:400\"><label class=mdl-textfield__label for=searchControl>Search for members...</label></div><div class=mdl-layout-spacer></div></div><div class=\"mdl-grid demo-content\"><div class=mdl-layout-spacer></div><span class=loading-text if.bind=!hasResults>No results found...</span><table membertrack-mdl=data-table class=mdl-shadow--2dp show.two-way=people.length><thead><tr><th>Name</th><th>Status</th><th>Age Group</th><th></th></tr></thead><tbody><tr repeat.for=\"person of people | sort : 'firstName'\"><td>${person.firstName} ${person.lastName}</td><td>${person.statusName}</td><td>${person.ageGroupName}</td><td><a membertrack-mdl=button class=mdl-button--accent href=#main/member-details/${person.id}>View Details</a></td></tr></tbody></table><div class=mdl-layout-spacer></div></div></section></template>"; });
define('text!views/sign-in.view.html', ['module'], function(module) { module.exports = "<template><section><div class=\"mdl-grid demo-content\"><div class=mdl-layout-spacer></div><div class=\"demo-cards mdl-cell mdl-cell--4-col mdl-cell--4-col-tablet mdl-grid mdl-grid--no-spacing\"><div class=\"demo-updates mdl-card mdl-shadow--2dp mdl-cell mdl-cell--4-col mdl-cell--4-col-tablet mdl-cell--12-col-desktop\"><div class=\"mdl-card__title mdl-card--expand mdl-color--teal-300\"><h2 class=mdl-card__title-text>Sign In</h2></div><form role=form submit.delegate=signIn($event)><div class=\"mdl-card__supporting-text mdl-color-text--grey-600\"><div membertrack-mdl=textfield class=\"mdl-textfield--floating-label full-width\"><input class=mdl-textfield__input type=email id=emailControl name=emailControl value.bind=model.email required><label class=mdl-textfield__label for=emailControl>Email...</label></div><div membertrack-mdl=textfield class=\"mdl-textfield--floating-label full-width\"><input class=mdl-textfield__input type=password id=passwordControl name=passwordControl value.bind=model.password required><label class=mdl-textfield__label for=passwordControl>Password...</label></div></div><div class=\"mdl-card__actions mdl-card--border\"><input membertrack-mdl=button type=submit id=sign-in-btn class=\"mdl-button--colored mdl-js-ripple-effect\" value=GO!></div></form></div></div><div class=mdl-layout-spacer></div></div></section></template>"; });
define('text!views/user-dialog.view.html', ['module'], function(module) { module.exports = "<template><dialog id=user-dialog class=mdl-dialog><form role=form submit.delegate=save($event)><h6>${model.id ? 'Edit' : 'Add'} User</h6><hr><div class=mdl-dialog__content><div membertrack-mdl=textfield class=\"mdl-textfield--floating-label full-width\"><input class=mdl-textfield__input type=email id=emailControl name=emailControl value.bind=model.email required><label class=mdl-textfield__label for=emailControl>Email...</label></div><div membertrack-mdl=textfield class=\"mdl-textfield--floating-label full-width\"><input class=mdl-textfield__input type=text id=displayNameControl name=displayNameControl value.bind=model.displayName required><label class=mdl-textfield__label for=displayNameControl>Display Name...</label></div><div membertrack-mdl=selectfield class=\"mdl-selectfield--floating-label full-width\"><select id=roleControl name=roleControl class=mdl-selectfield__select value.bind=model.role required><option model.bind=null>Select role...</option><option repeat.for=\"role of roles | sort : 'name'\" model.bind=role.id>${role.name}</option></select><label class=mdl-selectfield__label for=roleControl>Role...</label></div></div><hr><div class=mdl-dialog__actions><button membertrack-mdl=button type=button click.trigger=cancel()>Cancel</button><button membertrack-mdl=button type=submit class=mdl-button--accent>Save</button></div></form></dialog></template>"; });
define('text!views/users.view.html', ['module'], function(module) { module.exports = "<template><require from=../view-models/prompt-delete-dialog.view-model></require><require from=../view-models/user-dialog.view-model></require><membertrack-prompt-delete-dialog view-model.ref=promptDeleteDialogVm dismiss.delegate=dismissPromptDeleteDialog($event)></membertrack-prompt-delete-dialog><membertrack-user-dialog view-model.ref=userDialogVm dismiss.delegate=dismissUserDialog($event)></membertrack-user-dialog><section membertrack-view=users><div class=\"mdl-grid demo-content\"><div class=mdl-layout-spacer></div><div class=loading-text>${loadingText}</div><table membertrack-mdl=data-table class=mdl-shadow--2dp show.two-way=users.length><thead><tr><th>Name</th><th>Email</th><th>Role</th><th></th></tr></thead><tbody><tr repeat.for=\"user of users | sort : 'displayName'\"><td>${user.displayName}</td><td>${user.email}</td><td>${user.roleName}</td><td><button membertrack-mdl=button class=mdl-button--accent click.trigger=displayUserDialog(user)>Edit</button><button membertrack-mdl=button click.trigger=displayPromptDeleteDialog(user)>Delete</button></td></tr></tbody></table><div class=mdl-layout-spacer></div></div><button membertrack-mdl=button type=button class=\"mdl-button--raised mdl-js-ripple-effect mdl-button--primary add-btn\" click.trigger=displayUserDialog()>New User</button></section></template>"; });
define('text!resources/elements/snackbar.html', ['module'], function(module) { module.exports = "<template><div membertrack-mdl=snackbar><div class=mdl-snackbar__text></div><button class=mdl-snackbar__action type=button></button></div></template>"; });
//# sourceMappingURL=app-bundle.js.map