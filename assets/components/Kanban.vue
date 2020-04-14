<template>
    <div class="row mt-3">
        <div class="col-md-3">
            <p>{{ projectTest }}</p>
            <div class="p-2 alert alert-secondary" onmouseover="" style="cursor: pointer;">
                <h5>Backlog</h5>
                <draggable class="list-group kanban-column" :list="arrBacklog" group="tasks">
                    <div class="card card-kanban" v-for="element in arrBacklog" :key="element.name">
                        <div class="card-body">
                            <div class="dropdown card-options">
                                <button class="btn-options" type="button" id="kanban-dropdown-button-14" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    <i class="material-icons">more_vert</i>
                                </button>
                                <div class="dropdown-menu dropdown-menu-right">
                                    <a class="dropdown-item" href="#">Edit</a>
                                    <a class="dropdown-item text-danger" href="#">Archive Card</a>
                                </div>
                            </div>
                            <div class="card-title">
                                <a href="#" data-toggle="modal" data-target="#task-modal">
                                    {{ element.name }}
                                </a>
                            </div>

                            <ul class="avatars">
                                <li>
                                    <a href="#" data-toggle="tooltip" title="David">
                                        <!--<img alt="David Whittaker" class="avatar" src="~/assets/img/defaultUserIcon.jpg" />-->
                                    </a>
                                </li>

                                <li>
                                    <a href="#" data-toggle="tooltip" title="Ravi">
                                        <!--<img alt="Ravi Singh" class="avatar" src="~/assets/img/defaultUserIcon.jpg" />-->
                                    </a>
                                </li>

                            </ul>

                            <div class="card-meta d-flex justify-content-between">
                                <div class="d-flex align-items-center">
                                    <i class="material-icons">playlist_add_check</i>
                                    <span>-/-</span>
                                </div>

                                <span class="text-small">Unscheduled</span>

                            </div>

                        </div>
                    </div>
                </draggable>
            </div>
        </div>

        <div class="col-md-3">
            <div class="p-2 alert alert alert-secondary" onmouseover="" style="cursor: pointer;">
                <h5>In Progress</h5>
                <draggable class="list-group kanban-column" :list="arrInProgress" group="tasks">
                    <div class="card card-kanban" v-for="element in arrInProgress" :key="element.name" style="background-color: #defeff">
                        <div class="card-body">
                            <div class="dropdown card-options">
                                <button class="btn-options" type="button" id="kanban-dropdown-button-14" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    <i class="material-icons">more_vert</i>
                                </button>
                                <div class="dropdown-menu dropdown-menu-right">
                                    <a class="dropdown-item" href="#">Edit</a>
                                    <a class="dropdown-item text-danger" href="#">Archive Card</a>
                                </div>
                            </div>
                            <div class="card-title">
                                <a href="#" data-toggle="modal" data-target="#task-modal">
                                    <h6>{{element.name}}</h6>
                                </a>
                            </div>

                            <ul class="avatars">
                                <li>
                                    <a href="#" data-toggle="tooltip" title="David">
                                        <!--<img alt="David Whittaker" class="avatar" src="~/assets/img/defaultUserIcon.jpg" />-->
                                    </a>
                                </li>

                                <li>
                                    <a href="#" data-toggle="tooltip" title="Ravi">
                                        <!--<img alt="Ravi Singh" class="avatar" src="~/assets/img/defaultUserIcon.jpg" />-->
                                    </a>
                                </li>

                            </ul>

                            <div class="card-meta d-flex justify-content-between">
                                <div class="d-flex align-items-center">
                                    <i class="material-icons">playlist_add_check</i>
                                    <span>-/-</span>
                                </div>

                                <span class="text-small">Unscheduled</span>

                            </div>

                        </div>
                    </div>
                </draggable>
            </div>
        </div>

        <div class="col-md-3">
            <div class="p-2 alert alert alert-secondary" onmouseover="" style="cursor: pointer;">
                <h5>Testing</h5>
                <draggable class="list-group kanban-column" :list="arrTested" group="tasks">
                    <div class="card card-kanban" v-for="element in arrTested" :key="element.name" style="background-color: #fffede">
                        <div class="card-body">
                            <div class="dropdown card-options">
                                <button class="btn-options" type="button" id="kanban-dropdown-button-14" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    <i class="material-icons">more_vert</i>
                                </button>
                                <div class="dropdown-menu dropdown-menu-right">
                                    <a class="dropdown-item" href="#">Edit</a>
                                    <a class="dropdown-item text-danger" href="#">Archive Card</a>
                                </div>
                            </div>
                            <div class="card-title">
                                <a href="#" data-toggle="modal" data-target="#task-modal">
                                    <h6>{{element.name}}</h6>
                                </a>
                            </div>

                            <ul class="avatars">
                                <li>
                                    <a href="#" data-toggle="tooltip" title="David">
                                        <!--<<img alt="David Whittaker" class="avatar" src="~/assets/img/defaultUserIcon.jpg" />-->
                                    </a>
                                </li>

                                <li>
                                    <a href="#" data-toggle="tooltip" title="Ravi">
                                        <!--<img alt="Ravi Singh" class="avatar" src="~/assets/img/defaultUserIcon.jpg" />-->
                                    </a>
                                </li>

                            </ul>

                            <div class="card-meta d-flex justify-content-between">
                                <div class="d-flex align-items-center">
                                    <i class="material-icons">playlist_add_check</i>
                                    <span>-/-</span>
                                </div>

                                <span class="text-small">Unscheduled</span>

                            </div>

                        </div>
                    </div>
                </draggable>
            </div>
        </div>

        <div class="col-md-3">
            <div class="p-2 alert alert-secondary" onmouseover="" style="cursor: pointer;">
                <h5>Done</h5>
                <draggable class="list-group kanban-column" :list="arrDone" group="tasks">
                    <div class="card card-kanban" v-for="element in arrDone" :key="element.name" style="background-color: #e3ffde">
                        <div class="card-body">
                            <div class="dropdown card-options">
                                <button class="btn-options" type="button" id="kanban-dropdown-button-14" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    <i class="material-icons">more_vert</i>
                                </button>
                                <div class="dropdown-menu dropdown-menu-right">
                                    <a class="dropdown-item" href="#">Edit</a>
                                    <a class="dropdown-item text-danger" href="#">Archive Card</a>
                                </div>
                            </div>
                            <div class="card-title">
                                <a href="#" data-toggle="modal" data-target="#task-modal">
                                    <h6>{{element.name}}</h6>
                                </a>
                            </div>

                            <ul class="avatars">
                                <li>
                                    <a href="#" data-toggle="tooltip" title="David">
                                        <!--<img alt="David Whittaker" class="avatar" src="~/assets/img/defaultUserIcon.jpg" />-->
                                    </a>
                                </li>

                                <li>
                                    <a href="#" data-toggle="tooltip" title="Ravi">
                                        <!--<img alt="Ravi Singh" class="avatar" src="@y/assets/img/defaultUserIcon.jpg" />-->
                                    </a>
                                </li>

                            </ul>

                            <div class="card-meta d-flex justify-content-between">
                                <div class="d-flex align-items-center">
                                    <i class="material-icons">playlist_add_check</i>
                                    <span>-/-</span>
                                </div>

                                <span class="text-small">Unscheduled</span>

                            </div>

                        </div>
                    </div>
                </draggable>
            </div>
        </div>
    </div>
</template>

<script>
    import draggable from 'vuedraggable';

    export default {
        name: 'App',
        components: {
            draggable
        },
        props: ['projectTest'],
        data() {
            return {
                newTask: "",
                arrBacklog: [
                    { name: "Code Sign Up Page" },
                    { name: "Test dashboard" },
                    { name: "Build Kanban" },
                    { name: "Style Registration" },
                ],
                arrInProgress: [],
                arrTested: [],
                arrDone: []
            }
        },
        methods: {
            add() {
                if (this.newTask) {
                    this.arrBacklog.push({ name: this.newTask });
                    this.newTask = "";
                }
            }
        }
    }
</script>

<style>
    .kanban-column {
        min-height: 300px;
    }
</style>