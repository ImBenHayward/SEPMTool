export default{

    components: {
        draggable
    },
    data: function () {
        return {
            projectModel: @Json.Serialize(Model),

            totalTasks: 0,
            totalTasksCompleted: 0,

            objectIndex: 0,
            requirementIndex: 0,

            modalTitle: '',
            edit: 'false',

            requirementModalTitle: '',

            commentModel: {
                requirementId: '',
                parentId: '',
                parentComment: '',
                parentFName: '',
                parentLName: '',
                comment: ''
            },

            requirementModel: {
                id: '',
                name: '',
                description: '',
                category: '',
                priority: ''
            },

            taskModel: {
                id: 0,
                name: '',
                description: '',
                projectRequirementId: this.requirementId,
                projectRequirementIndex: '',
                progress: 0,
                subTasks: [],
                users: []
            },

            subTask: {
                description: '',
                isCompleted: false,
                //projectTaskId: '',
            },
            user: {
                userId: '',
                username: '',
                isSelected: false,
            },
            arrBacklog: [],
            arrInProgress: [],
            arrTested: [],
            arrDone: [],

            tree: [],

            optionalityIndicators: [
                {
                    word: 'possibly',
                    true: 0
                },
                {
                    word: 'eventually',
                    true: 0
                },
                {
                    word: 'if case',
                    true: 0
                },
                {
                    word: 'if possible',
                    true: 0
                },
                {
                    word: 'if appropriate',
                    true: 0
                },
                {
                    word: 'if needed',
                    true: 0
                },
            ],

            subjectivityIndicators: [
                {
                    word: 'similar',
                    true: 0
                },
                {
                    word: 'better',
                    true: 0
                },
                {
                    word: 'similarly',
                    true: 0
                },
                {
                    word: 'worse',
                    true: 0
                },
                {
                    word: 'having in mind',
                    true: 0
                },
                {
                    word: 'take into account',
                    true: 0
                }
            ],

            vaguenessIndicators: [
                {
                    word: 'clear',
                    true: 0
                },
                {
                    word: 'clearly',
                    true: 0
                },
                {
                    word: 'easy',
                    true: 0
                },
                {
                    word: 'easily',
                    true: 0
                },
                {
                    word: 'strong',
                    true: 0
                },
                {
                    word: 'good',
                    true: 0
                },
                {
                    word: 'bad',
                    true: 0
                },
                {
                    word: 'efficient',
                    true: 0
                },
                {
                    word: 'useful',
                    true: 0
                },
                {
                    word: 'significant',
                    true: 0
                },
                {
                    word: 'adequate',
                    true: 0
                },
                {
                    word: 'fast',
                    true: 0
                },
                {
                    word: 'recent',
                    true: 0
                },
                {
                    word: 'far',
                    true: 0
                },
                {
                    word: 'close',
                    true: 0
                },
                {
                    word: 'in front',
                    true: 0
                },
            ],

            weaknessIndicators: [
                {
                    word: 'can',
                    true: 0
                },
                {
                    word: 'could',
                    true: 0
                },
                {
                    word: 'may',
                    true: 0
                }
            ],

            implicityIndicators: [
                {
                    word: 'this',
                    true: 0
                },
                {
                    word: 'these',
                    true: 0
                },
                {
                    word: 'that',
                    true: 0
                },
                {
                    word: 'those',
                    true: 0
                },
                {
                    word: 'it',
                    true: 0
                },
                {
                    word: 'they',
                    true: 0
                },
                {
                    word: 'previous',
                    true: 0
                },
                {
                    word: 'next',
                    true: 0
                },
                {
                    word: 'following',
                    true: 0
                },
                {
                    word: 'last',
                    true: 0
                },
                {
                    word: 'above',
                    true: 0
                },
                {
                    word: 'below',
                    true: 0
                },
            ],

            multiplicityIndicators: [
                {
                    word: 'and',
                    true: 0
                },
                {
                    word: 'or',
                    true: 0
                },
                {
                    word: 'and/or',
                    true: 0
                }
            ],
        }
    },
    computed: {
        progressBarRounding: function () {
            if (task.progress === 100) {
                return 'border-radius: 320px'
            } else {
                return 'border-radius: .5rem 0px'
            }
        },

        totalIndicators: function () { return (this.optionalityIndicatorCount + this.subjectivityIndicatorCount + this.vaguenessIndicatorCount + this.weaknessIndicatorCount + this.implicityIndicatorCount + this.multiplicityIndicatorCount) },

        optionalityIndicatorCount: function () { return this.optionalityIndicators.filter(word => word.true).length },
        optionalityPercentage: function () { return parseFloat(this.optionalityIndicatorCount * 300 / this.wordCount) },

        subjectivityIndicatorCount: function () { return this.subjectivityIndicators.filter(word => word.true).length },
        subjectivityPercentage: function () { return parseFloat(this.subjectivityIndicatorCount * 300 / this.wordCount) },

        vaguenessIndicatorCount: function () { return this.vaguenessIndicators.filter(word => word.true).length },
        vaguenessPercentage: function () { return parseFloat(this.vaguenessIndicatorCount * 300 / this.wordCount) },

        weaknessIndicatorCount: function () { return this.weaknessIndicators.filter(word => word.true).length },
        weaknessPercentage: function () { return parseFloat(this.weaknessIndicatorCount * 300 / this.wordCount) },

        implicityIndicatorCount: function () { return this.implicityIndicators.filter(word => word.true).length },
        implicityPercentage: function () { return parseFloat(this.implicityIndicatorCount * 300 / this.wordCount) },

        multiplicityIndicatorCount: function () { return this.multiplicityIndicators.filter(word => word.true).length },
        multiplicityPercentage: function () { return parseFloat(this.multiplicityIndicatorCount * 300 / this.wordCount) },

        wordCount() {
            return this.requirementModel.description.trim().split(/\s+/).length;
        },

        allArrays: function () { return [...this.optionalityIndicators, ...this.subjectivityIndicators, ...this.vaguenessIndicators, ...this.weaknessIndicators, ...this.implicityIndicators, ...this.multiplicityIndicators] },
    },
    methods: {

        //buildHierarchy: function(arry) {

        //    var roots = [], children = {};

        //    // find the top level nodes and hash the children based on parent
        //    for (var i = 0, len = arry.length; i < len; ++i) {
        //        var item = arry[i],
        //            p = item.parentId,
        //            target = !p ? roots : (children[p] || (children[p] = []));

        //        target.push({ value: item });
        //    }

        //    // function to recursively build the tree
        //    var findChildren = function(parent) {
        //        if (children[parent.value.id]) {
        //            parent.children = children[parent.value.id];
        //            for (var i = 0, len = parent.children.length; i < len; ++i) {
        //                findChildren(parent.children[i]);
        //            }
        //        }
        //    };

        //    // enumerate through to handle the case where there are multiple roots
        //    for (var i = 0, len = roots.length; i < len; ++i) {
        //        findChildren(roots[i]);
        //    }

        //    console.log(roots);

        //    return roots;
        //},

        hierarchy: function(data) {
            const tree = [];
            const childOf = {};
            data.forEach((item) => {
                const { id, parentId } = item;
                childOf[id] = childOf[id] || [];
                item.children = childOf[id];
                parentId ? (childOf[parentId] = childOf[parentId] || []).push(item) : tree.push(item);
            });

            console.log(tree);

            this.tree = tree;

            return tree;
        },

        /*
            * Progress Calculator
        */
        CalculateProgress: function () {

            var taskCount = 0;
            var tasksCompleted = 0;
            var taskPercentage = 0;

            this.projectModel.project.projectRequirements.forEach(requirement => {

                taskCount += requirement.tasks.length;

                requirement.tasks.forEach(task => {

                    var subTaskCount = task.subTasks.length;
                    var subTasksComplete = 0;
                    var taskPercent = 0;

                    if (task.isCompleted == true) {
                        taskPercentage += 100;
                        task.progress = 100;
                        tasksCompleted++;
                    } else {
                        task.subTasks.forEach(subtask => {
                            if (subtask.isCompleted == true) {
                                subTasksComplete++;
                            }
                        });

                        if (subTaskCount > 0) {
                            taskPercent = parseFloat(subTasksComplete * 100 / subTaskCount);
                        }

                        task.progress = taskPercent;
                        taskPercentage += taskPercent;
                    }
                });
            });

            taskPercentage = taskPercentage.toFixed();
            var projectPercentage = parseFloat(taskPercentage / taskCount);

            this.totalTasks = taskCount;
            this.totalTasksCompleted = tasksCompleted;

            axios({
                method: 'patch',
                url: '/Projects/UpdateProjectProgress',
                params: {
                    projectId: this.projectModel.project.id,
                    progress: projectPercentage
                }
            })
            .then(res => {
                this.projectModel.project.progress = projectPercentage;
            });
        },


        /*
            * Requirement Methods
            */

        //Open New Requirement Modal
        openNewRequirementModal: function () {

            this.requirementModel.category = '';
            this.requirementModel.name = '';
            this.requirementModel.description = '';
            this.requirementModel.id = '';
            this.requirementModel.priority = '';

            this.requirementModalTitle = 'New Requirement';
            this.detectWords();

            $('#requirement-modal').modal('show');

        },

        CreateRequirement() {
            this.loading = true;
            $('#requirement-modal').modal('hide');
            axios({
                method: 'post',
                url: '/Projects/CreateRequirement',
                headers: {
                    'Content-Type': 'application/json'
                },
                data: {
                    projectId: this.projectModel.project.id,
                    requirementName: this.requirementModel.name,
                    requirementDescription: this.requirementModel.description,
                    requirementPriority: this.requirementModel.priority,
                    requirementCategory: this.requirementModel.category
                }
            })
                .then(res => {
                    console.log(res.data);
                    this.projectModel.project.projectRequirements.push(res.data.requirement);
                    this.CalculateProgress();
                    this.RenderRequirements();
                })
                .catch(err => {
                    console.log(err.message);
                })
                .then(() => {
                    this.loading = false;
                });
        },

        // Open Edit Requirement Modal
        openEditRequirementModal: function (req, reqIndex) {

            this.requirementModalTitle = 'Edit Requirement';

            this.requirementModel = {
                id: req.id,
                name: req.name,
                description: req.description,
                priority: req.priority,
                category: req.category
            }

            this.requirementIndex = reqIndex;
            this.detectWords();

            $('#requirement-modal').modal('show');
        },

        closeModal: function () {
            $('#requirement-modal').modal('hide');

            this.resetArrays();
        },

        //Update Requirement (Axios Request)
        UpdateRequirement() {
            this.loading = true;
            $('#requirement-modal').modal('hide');
            axios({
                method: 'post',
                url: '/Projects/UpdateRequirement',
                headers: {
                    'Content-Type': 'application/json'
                },
                data: {
                projectId: this.projectModel.project.id,
                requirementId: this.requirementModel.id,
                requirementName: this.requirementModel.name,
                requirementDescription: this.requirementModel.description,
                requirementPriority: this.requirementModel.priority,
                requirementCategory: this.requirementModel.category
                }
            })
                .then(res => {

                    this.projectModel.project.projectRequirements.splice(this.requirementIndex, 1, res.data.requirement);

                    //requirement.name = res.data.requirement.name;
                    //requirement.description = res.data.requirement.description;
                    //requirement.priority = res.data.requirement.priority;
                    //requirement.category = res.data.requirement.category;

                })
                .catch(err => {
                    console.log(err.message);
                })
                .then(() => {
                    this.loading = false;
                });
        },

        DeleteRequirement(id, reqIndex) {

                this.$swal({
                    title: 'Are you sure?',
                    text: 'Deleting this requirement cannot be undone',
                    type: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Yes, Delete',
                    cancelButtonText: 'No, Keep it!',
                    showCloseButton: true,
                    showLoaderOnConfirm: true
                }).then((result) => {
                    if (result.value) {
                        this.$swal('Deleted', 'You successfully deleted the requirement', 'success')

                        this.loading = true;
                        axios.delete('/Projects/DeleteRequirement/', {
                            params: {
                                reqId: id,
                                projectId: this.projectModel.project.id
                            }
                        })
                            .then(res => {
                                console.log(res);
                                this.projectModel.project.projectRequirements.splice(reqIndex, 1);
                                this.RenderKanban();
                                this.CalculateProgress();
                            })
                            .catch(err => {
                                console.log(err.message);
                            })
                            .then(() => {
                                this.loading = false;
                            });
                    } else {
                        this.$swal.close();
                    }
                })
        },

        //Render Requirement Tab Blocks
        RenderRequirements: function () {
            const TabBlock = {
                s: {
                    animLen: 200
                },

                init: function () {
                    TabBlock.bindUIActions();
                    TabBlock.hideInactive();
                },

                bindUIActions: function () {
                    $('.tabBlock-tabs').on('click', '.tabBlock-tab', function () {
                        TabBlock.switchTab($(this));
                    });
                },

                hideInactive: function () {
                    var $tabBlocks = $('.tabBlock');

                    $tabBlocks.each(function (i) {
                        var
                            $tabBlock = $($tabBlocks[i]),
                            $panes = $tabBlock.find('.tabBlock-pane'),
                            $activeTab = $tabBlock.find('.tabBlock-tab.is-active');

                        $panes.hide();
                        $($panes[$activeTab.index()]).show();
                    });
                },

                switchTab: function ($tab) {
                    var $context = $tab.closest('.tabBlock');

                    if (!$tab.hasClass('is-active')) {
                        $tab.siblings().removeClass('is-active');
                        $tab.addClass('is-active');

                        TabBlock.showPane($tab.index(), $context);
                    }
                },

                showPane: function (i, $context) {
                    var $panes = $context.find('.tabBlock-pane');

                    $panes.slideUp(TabBlock.s.animLen);
                    $($panes[i]).slideDown(TabBlock.s.animLen);
                }
            };

            $(function () {
                TabBlock.init();
            });
        },

        /*
            * Comment Methods
        */

        OpenCommentsModal: function (reqId, parentId, commentBody, parentFName, parentLName) {

            this.commentModel.requirementId = reqId;
            this.commentModel.parentId = parentId;
            this.commentModel.parentComment = commentBody;
            this.commentModel.parentFName = parentFName;
            this.commentModel.parentLName = parentLName;

            console.log(reqId, parentId);

            $('#new-comment-modal').modal('show');

        },

        AddNewComment: function () {
            this.loading = true;

            console.log(this.projectModel.project.id);
            console.log(this.commentModel.requirementId);
            console.log(this.commentModel.comment);

            var formData = new FormData();

            formData.append('projectId', this.projectModel.project.id);
            formData.append('requirementId', this.commentModel.requirementId);
            formData.append('commentBody', this.commentModel.comment);
            formData.append('parentId', this.commentModel.parentId);

            $('#requirement-modal').modal('hide');
            axios({
                method: 'post',
                url: '/Projects/AddComment',
                headers: {'Content-Type': 'multipart/form-data' },
                data: formData
            })
                .then(res => {
                    console.log(res.data);
                })
                .catch(err => {
                    console.log(err.message);
                })
                .then(() => {
                    this.loading = false;
                });
        },

        /*
            * Task Methods
        */

        // Open New-Task Modal
        OpenModal: function (requirementId, index) {

            this.taskModel.id = 0;
            this.taskModel.name = '';
            this.taskModel.description = '';
            this.taskModel.projectRequirementId = requirementId;
            this.taskModel.projectRequirementIndex = index;
            this.taskModel.subTasks = [];
            this.taskModel.users = [];

            this.modalTitle = 'New Task';

            $('#task-add-modal').modal('show');

        },

        //Show Tasks (List)
        ShowTasks: function (requirementId) {

            const chevron = "#chevron-" + requirementId;
            const accordion = "#accordion-" + requirementId;

            console.log(this.projectModel);
            $(chevron).toggleClass("down");
            $(accordion).toggle("slow");
        },

        //Show Task (Singular Task)
        ShowTask: function (taskId) {
            console.log('hit' + taskId);
            const chevron = "#chevron-" + taskId;
            const task = "#task-" + taskId;

            console.log(this.projectModel);
            $(chevron).toggleClass("down");
            $(task).toggle("slow");
        },

        //Show Sub-Tasks (List)
        ShowSubTasks: function (taskId) {

            const chevron = "#subtask-chevron-" + taskId;
            const accordion = "#accordion-" + taskId;

            console.log(this.projectModel);
            $(chevron).toggleClass("down");
            $(accordion).toggle("slow");
        },

        //Add Sub-Task
        AddSubTask: function () {
            const clone = Object.assign({}, this.subTask)
            this.taskModel.subTasks.push(clone);
            this.subTask = {
                title: '',
                completed: false,
            };
        },

        //Create Task
        CreateTask() {
            console.log(this.projectModel);
            this.loading = true;
            $('#task-add-modal').modal('hide');
            axios({
                method: 'post',
                url: '/Projects/CreateTask',
                headers: {
                    'Content-Type': 'application/json'
                },
                data: {
                projectId: this.projectModel.project.id,
                requirementId: this.taskModel.projectRequirementId,
                taskName: this.taskModel.name,
                taskDescription: this.taskModel.description,
                subTasks: this.taskModel.subTasks,
                users: this.projectModel.users
                }
            })
                .then(res => {
                    console.log(res.data);
                    this.projectModel.project.projectRequirements[this.taskModel.projectRequirementIndex].tasks.push(res.data);
                    this.UpdateKanban(res.data);
                    this.CalculateProgress();

                    if ($("#accordion-" + res.data.projectRequirementId).is(":visible")) {
                    } else {
                        this.ShowTasks(res.data.projectRequirementId);
                    }

                    this.ShowTask(res.data.id);

                })
                .catch(err => {
                    console.log(err.message);
                })
                .then(() => {
                    this.loading = false;
                });
        },

        //Toggle Task Completion
        ToggleTaskComplete(task, taskIndex, reqIndex, isCompleted) {
            axios({
                method: 'patch',
                url: '/Projects/ToggleTaskComplete',
                params: {
                    taskId: task.id
                }
            })
            .then(res => {
                this.projectModel.project.projectRequirements[reqIndex].tasks[taskIndex].isCompleted = !isCompleted;
                this.CalculateProgress();
            })
        },

        //Edit Task (Populates Modal)
        EditTask(task, taskIndex, reqIndex) {
            this.objectIndex = taskIndex;
            this.requirementIndex = reqIndex;
            console.log(reqIndex);
            this.taskModel = {
                id: task.id,
                name: task.name,
                description: task.description,
                subTasks: task.subTasks,
                users: task.users
            }
            this.modalTitle = 'Edit Task';
            $('#task-add-modal').modal('show');
        },

        //Update Task (Axios Request)
        UpdateTask() {
            console.log(this.projectModel);
            this.loading = true;
            $('#task-add-modal').modal('hide');
            axios({
                method: 'post',
                url: '/Projects/UpdateTask',
                headers: {
                    'Content-Type': 'application/json'
                },
                data: {
                projectId: this.projectModel.project.id,
                requirementId: this.taskModel.projectRequirementId,
                taskId: this.taskModel.id,
                taskName: this.taskModel.name,
                taskDescription: this.taskModel.description,
                subTasks: this.taskModel.subTasks,
                users: this.projectModel.users
                }
            })
                .then(res => {
                    console.log(res.data);
                    console.log("req" + this.reqIndex);
                    console.log("obj" + this.objectIndex);

                    this.projectModel.project.projectRequirements[this.requirementIndex].tasks.splice(this.objectIndex, 1, res.data);
                    this.CalculateProgress();
                    this.RenderKanban();
                })
                .catch(err => {
                    console.log(err.message);
                })
                .then(() => {
                    this.loading = false;
                });
        },

        // Delete Task
        DeleteTask(id, taskIndex, reqIndex) {

                this.$swal({
                    title: 'Are you sure?',
                    text: 'Deleting this task cannot be undone',
                    type: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Yes, Delete',
                    cancelButtonText: 'No, Keep it!',
                    showCloseButton: true,
                    showLoaderOnConfirm: true
                }).then((result) => {
                    if (result.value) {
                        this.$swal('Deleted', 'You successfully deleted the task', 'success')

                        this.loading = true;
                        axios.delete('/Projects/DeleteTask/', {
                            params: {
                                taskId: id,
                                projectId: this.projectModel.project.id
                            }
                        })
                            .then(res => {
                                console.log(res);
                                this.projectModel.project.projectRequirements[reqIndex].tasks.splice(taskIndex, 1);
                                this.RenderKanban();
                                this.CalculateProgress();
                            })
                            .catch(err => {
                                console.log(err.message);
                            })
                            .then(() => {
                                this.loading = false;
                            });

                    } else {
                        this.$swal.close();
                    }
                })
        },

        // Toggle Sub-Task Completion
        subtaskToggle: function (subTaskId, subTaskIndex, taskIndex, reqIndex, isCompleted) {

            axios({
                method: 'patch',
                url: '/Projects/UpdateSubTask',
                params: {
                    taskId: subTaskId
                }
            })
                .then(res => {
                    console.log(res.data);
                    this.projectModel.project.projectRequirements[reqIndex].tasks[taskIndex].subTasks[subTaskIndex].isCompleted = !isCompleted;
                    this.CalculateProgress();
                });
        },

        // Moment time helper
        moment(...args) {
            return moment(...args);
        },
        GetDateFromNow(date) {
            return this.$moment(date).fromNow();
        },

        /*
            * Kanban Board Functions
            */

        // When a card is moved to a new column
        onAdd: function (event, status) {

            let id = event.item.getAttribute('data-id');

            axios({
                method: 'patch',
                url: '/Projects/KanbanMoveItem',
                data: {
                    Status: status,
                    TaskId: id
                }
            });
        },

        // Render The Kanban's Cards
        RenderKanban: function () {

            this.arrBacklog = [];
            this.arrInProgress = [];
            this.arrTested = [];
            this.arrDone = [];

            this.projectModel.project.projectRequirements.forEach(requirement => {
            requirement.tasks.forEach(task => {
                switch (task.status) {
                    case 0:
                        this.arrBacklog.push(task);
                        break;
                    case 1:
                        this.arrInProgress.push(task);
                        break;
                    case 2:
                        this.arrTested.push(task);
                        break;
                    case 3:
                        this.arrDone.push(task);
                        break;
                }
            });
        });
        },

        // Update the Kanban's Cards
        UpdateKanban: function (task) {
        switch (task.status) {
            case 0:
                console.log(task);
                this.arrBacklog.push(task);
                break;
            case 1:
                this.arrInProgress.push(task);
                break;
            case 2:
                this.arrTested.push(task);
                break;
            case 3:
                this.arrDone.push(task);
                break;
        }
        },

        /*
            * Word Search Functions
            */

        detectWords: function () {

            //For each word in the array
            this.optionalityIndicators.forEach(word => {
                if (this.wordInString(this.requirementModel.description, word.word)) {
                    word.true = 1;
                } else {
                    word.true = 0;
                }
            })

            this.subjectivityIndicators.forEach(word => {
                if (this.wordInString(this.requirementModel.description, word.word)) {
                    word.true = 1;
                } else {
                    word.true = 0;
                }
            })

            this.vaguenessIndicators.forEach(word => {
                if (this.wordInString(this.requirementModel.description, word.word)) {
                    word.true = 1;
                } else {
                    word.true = 0;
                }
            })

            this.weaknessIndicators.forEach(word => {
                if (this.wordInString(this.requirementModel.description, word.word)) {
                    word.true = 1;
                } else {
                    word.true = 0;

                    //if (this.subjectivityIndicators.every(word => !word.true)) {
                    //    console.log('yep');
                    //}
                }
            })

            this.implicityIndicators.forEach(word => {
                if (this.wordInString(this.requirementModel.description, word.word)) {
                    word.true = 1;
                } else {
                    word.true = 0;
                }
            })


            this.multiplicityIndicators.forEach(word => {
                if (this.wordInString(this.requirementModel.description, word.word)) {
                    word.true = 1;
                } else {
                    word.true = 0;
                }
            })

        },

        // searches for words in a string
        wordInString: function (s, word) {
            return new RegExp('\\b' + word + '\\b', 'i').test(s);
        },

        // Reset indicator arrays
        resetArrays: function () {
            this.allArrays.forEach(word => {
                word.true = 0;
            });
        },
    },
    beforeMount() {
        this.RenderKanban();
        this.CalculateProgress();
        this.RenderRequirements();
    },
}