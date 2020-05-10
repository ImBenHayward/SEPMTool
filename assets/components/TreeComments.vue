<template>
    <div class="media">
        <img class="mr-3 comments-img" src="http://placekitten.com/100/100" alt="Generic placeholder image">
        <div class="media-body">
            <h6 class="media-heading">{{firstName + " " + lastName}}<small><i> {{GetDateFromNow(dateTime)}}</i></small></h6>
            <div class="media-comment">{{ commentBody }}</div>
            <div class="media-buttons">
                <a href="javascript:;" class="badge badge-primary-light"><i class="far fa-thumbs-up"></i> 32</a>
                <a href="javascript:;" class="badge badge-primary-light open-comments-btn" v-on:click="EmitOpenCommentsModal(reqIndex, reqId, id, commentBody, firstName, lastName, userId)"><i class="fas fa-reply"></i> Reply</a>
                <a href="javascript:;" class="badge badge-danger-light delete-comments-btn" v-on:click="EmitDeleteComment(reqIndex, commentIndex, id)" v-if="currentUser == userId"><i class="fas fa-trash-alt"></i> Delete</a>
            </div>
            <!--<div ><i class="fas fa-reply comments-reply"></i> Reply</div>-->

            <tree-comments v-if="children.length && !maxDepthReached"
                           v-for="child in children"
                           :id="child.id"
                           :req-id="reqId"
                           :req-index="reqIndex"
                           :comment-index="commentIndex"
                           :user-id="child.userId"
                           :current-user="child.currentUser"
                           :first-name="child.firstName"
                           :last-name="child.lastName"
                           :date-time="child.dateTime"
                           :children="child.children || []"
                           :comment-body="child.commentBody"
                           :depth="depth + 1" />
        </div>
    </div>

</template>

<script>
    //$('.open-comments-btn').click(function ($e) {
    //    $e.preventDefault();
    //});

    //$('.delete-comments-btn').click(function ($e) {
    //    $e.preventDefault();
    //});
</script>

<script>
    const MAX_DEPTH = 99;

    export default {
        name: 'TreeComments',
        props: {
            id: Number,
            reqId: Number,
            reqIndex: Number,
            commentIndex: Number,
            userId: String,
            currentUser: String,
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
            EmitOpenCommentsModal: function (reqIndex, reqId, parentId, commentBody, parentFName, parentLName, userId) {

                var payload = {
                    reqIndex: this.reqIndex,
                    reqId: this.reqId,
                    parentId: this.id,
                    commentBody: this.commentBody,
                    parentFName: this.firstName,
                    parentLName: this.lastName,
                    userId: this.userId
                }

                this.$root.$emit('CommentsModal', payload);

            },

            EmitDeleteComment(reqIndex, reqId, id) {

                var payload = {
                    reqIndex: this.reqIndex,
                    commentIndex: this.commentIndex,
                    id: this.id
                }

                this.$root.$emit('DeleteComment', payload);

            },
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
