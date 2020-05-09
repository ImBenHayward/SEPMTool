<template>
    <div class="media">
        <img class="mr-3 comments-img" src="http://placekitten.com/100/100" alt="Generic placeholder image">
        <div class="media-body">

            <div class="media mt-3">
                <div class="media-body">
                    <h6 class="mt-0">{{firstName + " " + lastName}}<small><i> {{GetDateFromNow(dateTime)}}</i></small></h6>
                    {{ commentBody }}
                    <br />
                    <a href="#" class="badge badge-primary-outline"><i class="far fa-thumbs-up"></i> 32</a>
                    <div v-on:click="OpenCommentsModal2(reqId, id, commentBody, firstName, lastName)"><i class="fas fa-reply comments-reply"></i> Reply</div>

                    <tree-comments v-if="children.length && !maxDepthReached"
                                   v-for="child in children"
                                   :id="child.id"
                                   :req-id="reqId"
                                   :first-name="child.firstName"
                                   :last-name="child.lastName"
                                   :date-time="child.dateTime"
                                   :children="child.children || []"
                                   :comment-body="child.commentBody"
                                   :depth="depth + 1" />
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    const MAX_DEPTH = 99;

    export default {
        name: 'TreeComments',
        props: {
            id: Number,
            reqId: Number,
            commentBody: String,
            firstName: String,
            lastName: String,
            dateTime: null,
            children: Array,
            depth: {
                type: Number,
                default: 1,
            },
        },
        methods: {
            //moment(...args) {
            //    return moment(...args);
            //},
            GetDateFromNow(date) {
                return this.$moment(date).fromNow();
            },
        },
        computed: {
            maxDepthReached() {
                return this.depth >= MAX_DEPTH;
            }
        }
    };
</script>

<!--<template>
    <div class="tree-menu">
        <div>hello</div>
        <tree-menu v-for="node in nodes"
                   :nodes="node.nodes"
                   :label="node.label">
        </tree-menu>
    </div>
</template>

<script>
  export default {
    props: [ 'label', 'nodes' ],
    name: 'child-component'
  }
</script>-->
