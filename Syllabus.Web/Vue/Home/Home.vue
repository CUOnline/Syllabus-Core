<template>
    <div>
        <img id="logo" src="images/cu-online.png" />
        <div id="content" v-show="!isLoading">

            <h2>Syllabus Exporter</h2>
            <a href="webUrl" target="_blank">https://ucdenver.instructure.com</a>
            <br />
            <a href="/Home/ExternalLogout" class="btn btn-sm btn-secondary float-right">Log Out</a>
            
            <hr />

            <div class="row">
                <form class="form-inline" style="width: 100%">
                    <div class="form-group">
                        <label>Search Term</label>
                        <input class="form-control" v-model="searchTerm" />
                    </div>
                    <div class="form-group">
                        <label>Department</label>
                        <select v-model="selectedDepartment">
                            <option v-for="dept in departments">
                                {{ dept }}
                            </option>
                        </select>
                    </div>
                    <div class="form-group">
                        <label>Term</label>
                        <select v-model="selectedTerm">
                            <option v-for="term in terms" v-bind:value="term.id">
                                {{ term.name }}
                            </option>
                        </select>
                    </div>
                    <input type="button" class="btn btn-sm btn-cu" value="Submit" @click="search()" />
                </form>
            </div>

            <hr />

            <div v-show="searchRun">
                <div class="row">
                    <div class="col">
                        <form class="form-inline" style="width:100%">
                            <div class="form-group">
                                <input type="checkbox" v-model="selectAllCheck" @click="selectAll()" />
                                <label class="form-check-label">Select All</label>
                            </div>
                            <div class="form-group">
                                <input type="checkbox" v-model="hideMissingSyllabi" />
                                <label class="form-check-label">Hide Missing Syllabi</label>
                            </div>
                            <input type="button" class="btn btn-sm btn-cu" value="Export Selected" @click="exportSelected()" />
                        </form>
                    </div>
                    <div class="col">
                        <span class="float-right" v-show="searchResults.length > 0">
                            {{searchResults.length}} results
                        </span>
                    </div>
                </div>

                <div class="row">
                    <table>
                        <thead>
                            <tr class="even">
                                <th>Select</th>
                                <th>Course Name</th>
                                <th>Course Code</th>
                                <th>Canvas ID</th>
                                <th>View Syllabus</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="result in searchResults" :class="{'result-row': true }" v-show="result.syllabusBody != null || !hideMissingSyllabi">
                                <td>
                                    <input type="checkbox" class="export-checkbox" v-model="result.export" />
                                </td>
                                <td>
                                    <a v-bind:href="'http://ucdenver.instructure.com/courses/' + result.canvasId" target="_blank">{{result.name}}</a>
                                </td>
                                <td>
                                    {{result.code}}
                                </td>
                                <td>
                                    {{result.canvasId}}
                                </td>
                                <td v-if="result.syllabusBody != null">
                                    <span class="btn-link" style="cursor:pointer" @click="openSyllabus(result.syllabusBody)">View Syllabus</span>
                                </td>
                                <td v-else>
                                    Missing or Empty
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    import Vue from "vue";
    import axios from "axios";
    import _ from "lodash";
    import filedownload from "js-file-download";

    export default Vue.extend({
        props: {
        },
        data() {
            return {
                selectAllCheck: false,
                hideMissingSyllabi: true,
                searchRun: false,
                loadingData: false,
                isLoading: false,
                isSaving: false,
                error: "",
                searchTerm: '',
                selectedDepartment: 'AAS',
                departments: [
                    'AAS', 'ACCT', 'AGRI', 'ANAT', 'ANEQ', 'ANES', 'ANMS', 'ANTH', 'ANTP', 'ARAB',
                    'ARCH', 'ARTH', 'ARTS', 'BANA', 'BIOE', 'BIOL', 'BIOS', 'BLAW', 'BMIN', 'BUSN',
                    'CANB', 'CAND', 'CBHS', 'CCDI', 'CCDM', 'CHBH', 'CHEM', 'CHIN', 'CLDE', 'CLSC',
                    'CMDT', 'CNCR', 'COMM', 'CPBS', 'CPCE', 'CRJU', 'CSCI', 'CSDV', 'CVEN', 'DERM',
                    'DISP', 'DPER', 'DPTR', 'DSAD', 'DSBS', 'DSEL', 'DSEN', 'DSEP', 'DSFD', 'DSGD',
                    'DSOD', 'DSON', 'DSOP', 'DSOR', 'DSOS', 'DSOT', 'DSPD', 'DSPE', 'DSPL', 'DSRE',
                    'DSRP', 'DSSD', 'ECED', 'ECON', 'EDFN', 'EDHD', 'EDLI', 'EDRM', 'EDUC', 'EHOH',
                    'ELEC', 'ELED', 'EMED', 'ENGL', 'ENGR', 'ENTP', 'ENVS', 'EPID', 'EPSY', 'ERHS',
                    'ETHS', 'ETST', 'FILM', 'FINE', 'FITV', 'FMMD', 'FNCE', 'FREN', 'FSHN', 'GEMM',
                    'GENC', 'GEOG', 'GEOL', 'GERO', 'GRMN', 'HBSC', 'HDFR', 'HDFS', 'HESC', 'HIPR',
                    'HIS', 'HIST', 'HLTH', 'HMGP', 'HPL', 'HSMP', 'HUMN', 'IDPT', 'IEOO', 'IMMU',
                    'INTB', 'INTE', 'INTS', 'IPED', 'IPHY', 'ISMG', 'ITED', 'IWKS', 'JTCM', 'LATN',
                    'LCRT', 'LDAR', 'MATH', 'MCKE', 'MECH', 'MEDS', 'MGMT', 'MICB', 'MILR', 'MINS',
                    'MIPO', 'MKTG', 'MLNG', 'MOLB', 'MPAS', 'MSRA', 'MTAX', 'MTED', 'MTH', 'MU',
                    'MUSC', 'NCBE', 'NCCM', 'NCED', 'NCEG', 'NCES', 'NCMA', 'NEUR', 'NRSC', 'NSUR',
                    'NUDO', 'NURS', 'OBGY', 'OPHT', 'ORTH', 'OTOL', 'PATH', 'PBHC', 'PBHL', 'PEDS',
                    'PHCL', 'PHIL', 'PHLY', 'PHMD', 'PHRD', 'PHSC', 'PHYS', 'PMUS', 'PRDI', 'PRDO',
                    'PRMD', 'PSCI', 'PSCY', 'PSYC', 'PSYM', 'PUAD', 'PUBH', 'RADI', 'RAON', 'RHSC',
                    'RISK', 'RLST', 'RPSC', 'RSEM', 'SCHL', 'SECE', 'SJUS', 'SOCO', 'SOCY', 'SPAN',
                    'SPCM', 'SPED', 'SPSY', 'SRMS', 'SSCI', 'STBB', 'STDY', 'SURG', 'SUST', 'TCED',
                    'THTR', 'TLED', 'TXCL', 'UEDU', 'UNHL', 'URBN', 'URPL', 'VSCS', 'WGST', 'XBUS',
                    'XHAD'],
                selectedTerm: "",
                terms: [],
                searchResults: []
            }
        },
        mounted() {
            this.isLoading = true;

            axios.get('/api/Term')
                .then(res => {
                    this.terms = res.data;
                    this.selectedTerm = res.data[0].id;
                    this.isLoading = false;
                })
                .catch((error) => {
                    this.error = error;
                    this.isSaving = false;
                });

        },
        methods: {
            selectAll() {
                for (let i = 0; i < this.searchResults.length; ++i) {
                    this.searchResults[i].export = (this.searchResults[i].syllabusBody != null || !this.hideMissingSyllabi) && !this.selectAllCheck;
                }
            },
            openSyllabus(html) {
                var newWindow = window.open();
                html = html.replace(/\\n/g, "<br />");
                newWindow.document.write(html);
            },
            exportSelected() {
                var dto = this.searchResults.filter(x => x.export);
                axios.post('/Home/ExportSyllabi', dto, {responseType: 'blob'}  )
                    .then(res => {
                        filedownload(res.data, 'export.zip');
                    })
                    .catch((error) => {
                        this.error = error;
                        this.isSaving = false;
                    });
            },
            search() {
                this.searchRun = true;

                let dto = {
                    searchTerm: this.searchTerm,
                    department: this.selectedDepartment,
                    selectedTerm: this.selectedTerm
                };

                axios.post('/api/Search', dto)
                    .then(res => {
                        this.searchResults = res.data;
                    })
                    .catch((error) => {
                        this.error = error;
                        this.isSaving = false;
                    });
            }
        },
    });
</script>