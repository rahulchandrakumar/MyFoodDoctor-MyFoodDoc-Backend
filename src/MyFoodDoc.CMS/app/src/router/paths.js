import UserRoles from "@/enums/UserRoles";

export default [
    {
        path: "/login",
        name: "Login",
        view: "Login",
        meta: {
            requiresAuth: false
        }
    },
    {
        path: "",
        // Relative to /src/views
        view: "Dashboard",
        meta: {
            role: UserRoles.VIEWER
        }
    },
    {
        path: "/user-profile",
        name: "User Profile",
        view: "UserProfile",
        meta: {
            role: UserRoles.VIEWER
        }
    },
    {
        path: "/portion-list",
        name: "Portions",
        view: "Data/PortionList",
        meta: {
            role: UserRoles.EDITOR
        }
    },
    {
        path: "/patient-list",
        name: "Patients",
        view: "Data/PatientList",
        meta: {
            role: UserRoles.VIEWER
        }
    },
    {
        path: "/lexicon-category-list",
        name: "Lexicon Categories",
        view: "Data/LexiconCategoryList",
        meta: {
            role: UserRoles.ADMIN
        }
    },
    {
        path: "/lexicon-list/:parentId",
        name: "Lexicon",
        view: "Data/LexiconList",
        meta: {
            role: UserRoles.ADMIN
        }
    },
    {
        path: "/optimizationarea-list",
        name: "Optimization Areas",
        view: "Data/OptimizationAreaList",
        meta: {
            role: UserRoles.ADMIN
        }
    },
    {
        path: "/target-list/:parentId",
        name: "Targets",
        view: "Data/TargetList",
        meta: {
            role: UserRoles.ADMIN
        }
    },
    {
        path: "/method-list",
        name: "Methods",
        view: "Data/MethodList",
        meta: {
            role: UserRoles.ADMIN
        }
    },
    {
        path: "/methodmultiplechoice-list/:parentId",
        name: "Method Multiple Choices",
        view: "Data/MethodMultipleChoiceList",
        meta: {
            role: UserRoles.ADMIN
        }
    },
    {
        path: "/course-list",
        name: "Courses",
        view: "Data/CourseList",
        meta: {
            role: UserRoles.ADMIN
        }
    },
    {
        path: "/chapter-list/:parentId",
        name: "Chapters",
        view: "Data/ChapterList",
        meta: {
            role: UserRoles.ADMIN
        }
    },
    {
        path: "/subchapter-list/:parentId",
        name: "Subchapters",
        view: "Data/SubchapterList",
        meta: {
            role: UserRoles.ADMIN
        }
    },
    {
        path: "/webview-list",
        name: "WebView List",
        view: "Data/WebViewList",
        meta: {
            role: UserRoles.ADMIN
        }
    },
    {
        path: "/promotion-list",
        name: "Promotions",
        view: "Data/PromotionList",
        meta: {
            role: UserRoles.EDITOR
        }
    },
    {
        path: "/users",
        name: "Users",
        view: "Users",
        meta: {
            role: UserRoles.ADMIN
        }
    },
    {
        path: "/typography",
        view: "Typography",
        meta: {
            role: UserRoles.ADMIN
        }
    },
    {
        path: "/icons",
        view: "Icons",
        meta: {
            role: UserRoles.ADMIN
        }
    },
    {
        path: "/maps",
        view: "Maps",
        meta: {
            role: UserRoles.ADMIN
        }
    },
    {
        path: "/notifications",
        view: "Notifications",
        meta: {
            role: UserRoles.ADMIN
        }
    },
    {
        path: "/logout"
    }
];
