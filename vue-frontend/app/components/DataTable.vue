<script setup lang="ts" generic="TData, TValue">
import { Plus, Search } from "lucide-vue-next";
import type { ColumnDef, Row, SortingState } from '@tanstack/vue-table'
import {
  FlexRender,
  getCoreRowModel,
  getFilteredRowModel,
  getSortedRowModel,
  useVueTable,
} from '@tanstack/vue-table'

import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table'
import { valueUpdater } from '~/lib/utils';

const {
  columns,
  showNew,
  showSearch = true,
  newLink,
  data,
  rowClickHandler = null,
} = defineProps<{
  columns: ColumnDef<TData, TValue>[],
  showNew: boolean,
  showSearch?: boolean | undefined,
  newLink: string,
  data: TData[],
  rowClickHandler?: (row: Row<TData>) => void,
}>()

const sorting = ref<SortingState>([]);
// eslint-disable-next-line @typescript-eslint/no-explicit-any
const globalFilter = ref<any>([]);

const table = useVueTable({
  get data() { return data },
  get columns() { return columns },
  getCoreRowModel: getCoreRowModel(),
  getFilteredRowModel: getFilteredRowModel(),
  globalFilterFn: 'includesString',
  getSortedRowModel: getSortedRowModel(),
  onSortingChange: updaterOrValue => valueUpdater(updaterOrValue, sorting),
  onGlobalFilterChange: updaterOrValue => valueUpdater(updaterOrValue, globalFilter),
  state: {
    get globalFilter() { return globalFilter.value; },
    get sorting() { return sorting.value; },
  }
})

</script>

<template>
  <div class="space-y-4">
    <div class="flex flex-wrap justify-between gap-4">
      <div v-if="showSearch" class="relative flex-grow-1 max-w-sm items-center">
        <Input id="search" type="text" placeholder="Search..." class="pl-10" :model-value="globalFilter"
          @update:model-value="table.setGlobalFilter" />
        <span class="absolute start-0 inset-y-0 flex items-center justify-center px-2">
          <Search class="size-4 text-muted-foreground" />
        </span>
      </div>
      <div v-else />
      <Button v-if="showNew" type="button" as-child>
        <NuxtLink :to="newLink">
          <Plus /> New
        </NuxtLink>
      </Button>
    </div>
    <div class="border rounded-md">
      <Table>
        <TableHeader>
          <TableRow v-for="headerGroup in table.getHeaderGroups()" :key="headerGroup.id">
            <TableHead v-for="header in headerGroup.headers" :key="header.id">
              <FlexRender v-if="!header.isPlaceholder" :render="header.column.columnDef.header"
                :props="header.getContext()" />
            </TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          <template v-if="table.getRowModel().rows?.length">
            <TableRow v-for="row in table.getRowModel().rows" :key="row.id"
              :class="rowClickHandler ? 'cursor-pointer' : null"
              :data-state="row.getIsSelected() ? 'selected' : undefined"
              @click="rowClickHandler ? rowClickHandler(row) : null">
              <TableCell v-for="cell in row.getVisibleCells()" :key="cell.id">
                <FlexRender :render="cell.column.columnDef.cell" :props="cell.getContext()" />
              </TableCell>
            </TableRow>
          </template>
          <template v-else>
            <TableRow>
              <TableCell :colspan="columns.length" class="h-24 text-center">
                No results.
              </TableCell>
            </TableRow>
          </template>
        </TableBody>
      </Table>
    </div>
  </div>
</template>